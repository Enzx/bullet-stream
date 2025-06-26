using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletSteam.GameFramework.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BulletSteam.GameFramework.AssetManagement
{
    /// <summary>
    /// Service for loading assets and scenes using Unity's Addressables system.
    /// </summary>
    public class AssetLoaderService : ServiceBase
    {
        /// <summary>
        /// Singleton class that provides equality comparison for IResourceLocation based on PrimaryKey.
        /// </summary>
        public class LocationEqualityComparer : IEqualityComparer<IResourceLocation>
        {
            public static LocationEqualityComparer Instance { get; } = new();

            private LocationEqualityComparer()
            {
            }

            public bool Equals(IResourceLocation x, IResourceLocation y)
            {
                return x != null && y != null && x.PrimaryKey == y.PrimaryKey;
            }

            public int GetHashCode(IResourceLocation obj)
            {
                return obj.PrimaryKey.GetHashCode();
            }
        }

        /// <summary>
        /// Using this to get assets for the right AB test group.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string TestGroup { get; set; }

        private IResourceLocator _locator;

        public override void OnBuild(ServiceLocator<IService> locator)
        {
            _ = Initialize();
        }

        /// <summary>
        /// Initializes the Addressables system and returns the resource locator.
        /// </summary>
        /// <returns>The initialized IResourceLocator.</returns>
        public async Task<IResourceLocator> Initialize()
        {
            IResourceLocator locator = await Addressables.InitializeAsync().Task;
            _locator = locator;

            return locator;
        }


        /// <summary>
        ///  Instantiates an asset asynchronously.
        /// </summary>
        /// <param name="key"> The key of the asset to instantiate.</param>
        /// <returns> The instantiated GameObject.</returns>
        public async Task<TComponent> InstantiateAssetAsync<TComponent>(string key) where TComponent: Component
        {
            List<IResourceLocation> locations = GetResourceLocations<GameObject>(key);
            if (locations.Count == 0)
            {
                Debug.LogError($"No asset found with key {key}");
                return null;
            }

            GameObject gameObject = await Addressables.InstantiateAsync(locations[0]).Task;
            TComponent component = gameObject.GetComponent<TComponent>();
            if (component != null) return component;
            Debug.LogError($"No component of type {typeof(TComponent)} found on instantiated GameObject");
            return null;
        }

        /// <summary>
        /// Releases an instantiated GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to release.</param>
        /// <returns>Whether the release was successful.</returns>
        public bool Release(GameObject gameObject)
        {
            return Addressables.ReleaseInstance(gameObject);
        }

        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="key">The key of the scene to load.</param>
        /// <param name="mode">The LoadSceneMode.</param>
        /// <param name="activateOnLoad">Whether to activate the scene upon loading.</param>
        /// <param name="progressCallback">A callback to report progress.</param>
        /// <returns>The loaded SceneInstance.</returns>
        public async Task<SceneInstance> LoadSceneAsync(string key, LoadSceneMode mode = LoadSceneMode.Single,
            bool activateOnLoad = true, Action<float> progressCallback = null)
        {
            List<IResourceLocation> locations = GetSceneResourceLocations(key);
            if (locations.Count == 0)
            {
                Debug.LogError($"No scene found with key {key}");
                return default;
            }
            
            IResourceLocation location = locations[0];
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(location, mode, activateOnLoad);
     
            while (!handle.IsDone)
            {
                // Report progress
                progressCallback?.Invoke(handle.PercentComplete * 100);
                await Task.Yield();
            }
            
            SceneInstance scene = await handle.Task;
            return scene;
        }

        /// <summary>
        /// Loads multiple scenes asynchronously.
        /// </summary>
        /// <param name="keys">The keys of the scenes to load.</param>
        /// <param name="mode">The LoadSceneMode.</param>
        /// <param name="activateOnLoad">Whether to activate the scenes upon loading.</param>
        /// <returns>A list of loaded SceneInstances.</returns>
        public async Task<List<SceneInstance>> LoadScenesAsync(List<string> keys,
            LoadSceneMode mode = LoadSceneMode.Additive,
            bool activateOnLoad = true)
        {
            List<IResourceLocation> locations = new();
            foreach (string key in keys)
            {
                List<IResourceLocation> loc = GetSceneResourceLocations(key);
                locations.AddRange(loc);
            }

            if (locations.Count == 0)
            {
                Debug.LogError($"No scenes found with keys {string.Join(",", keys)}");
                return null;
            }

            List<Task<SceneInstance>> tasks = new();
            foreach (IResourceLocation location in locations)
            {
                tasks.Add(Addressables.LoadSceneAsync(location, mode, activateOnLoad).Task);
            }

            await Task.WhenAll(tasks);
            List<SceneInstance> scenes = tasks.Select(t => t.Result).ToList();

            return scenes;
        }

        /// <summary>
        /// Loads an asset asynchronously by key.
        /// </summary>
        /// <typeparam name="T">Type of the asset.</typeparam>
        /// <param name="key">Key of the asset to load.</param>
        /// <returns>The loaded asset of type T.</returns>
        public async Task<T> LoadAssetAsync<T>(string key) where T : Object
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Key is null or empty");
                return null;
            }

            List<IResourceLocation> locations = GetResourceLocations<T>(key);
            if (locations.Count == 0)
            {
                Debug.LogError($"No asset found with key {key}");
                return null;
            }

            IResourceLocation location = locations[0];
            T asset = await Addressables.LoadAssetAsync<T>(location).Task;
            return asset;
        }


        /// <summary>
        /// Loads multiple assets asynchronously by keys.
        /// </summary>
        /// <typeparam name="T">Type of the assets.</typeparam>
        /// <param name="keys">List of keys of the assets to load.</param>
        /// <returns>A list of loaded assets of type T.</returns>
        public async Task<List<T>> LoadAssetsAsync<T>(List<string> keys) where T : Object
        {
            List<IResourceLocation> locations = new();
            foreach (string key in keys)
            {
                List<IResourceLocation> loc = GetResourceLocations<T>(key);
                locations.AddRange(loc);
            }

            if (locations.Count == 0)
            {
                Debug.LogError($"No assets found with keys {string.Join(",", keys)}");
                return null;
            }

            List<Task<T>> tasks = new();
            foreach (IResourceLocation location in locations)
            {
                tasks.Add(Addressables.LoadAssetAsync<T>(location).Task);
            }

            await Task.WhenAll(tasks);
            List<T> assets = tasks.Select(t => t.Result).ToList();

            return assets;
        }

        /// <summary>
        /// Gets the resource locations for a given key and type, filtered by the test group.
        /// </summary>
        /// <typeparam name="T">Type of the resources.</typeparam>
        /// <param name="key">Key of the resource.</param>
        /// <returns>A list of resource locations.</returns>
        private List<IResourceLocation> GetResourceLocations<T>(string key) where T : Object
        {
            if (!_locator.Locate(key, typeof(T), out IList<IResourceLocation> locations))
            {
                Debug.LogError($"No resource locations found for key: {key}");
                return new List<IResourceLocation>();
            }

            if (string.IsNullOrEmpty(TestGroup))
            {
                return locations.ToList();
            }

            if (!_locator.Locate(TestGroup, typeof(T), out IList<IResourceLocation> groups))
            {
                Debug.LogError($"No resource locations found for test group: {TestGroup}");
                return new List<IResourceLocation>();
            }

            List<IResourceLocation> distinct = groups.Intersect(locations, LocationEqualityComparer.Instance).ToList();
            return distinct;
        }

        /// <summary>
        /// Gets the resource locations for a scene by key, filtered by the test group.
        /// </summary>
        /// <param name="key">Key of the scene.</param>
        /// <returns>A list of scene resource locations.</returns>
        private List<IResourceLocation> GetSceneResourceLocations(string key)
        {
            if (!_locator.Locate(key, null, out IList<IResourceLocation> locations))
            {
                Debug.LogError($"No resource locations found for key: {key}");
                return new List<IResourceLocation>();
            }
            if (string.IsNullOrEmpty(TestGroup))
            {
                return locations.ToList();
            }
            
            if (!_locator.Locate(TestGroup, null, out IList<IResourceLocation> groups))
            {
                Debug.LogError($"No resource locations found for test group: {TestGroup}");
                return new List<IResourceLocation>();
            }

            List<IResourceLocation> distinct = groups.Intersect(locations, LocationEqualityComparer.Instance).ToList();
            return distinct;
        }
    }
}