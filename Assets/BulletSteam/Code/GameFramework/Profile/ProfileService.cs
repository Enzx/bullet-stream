using System.Collections.Generic;
using BulletSteam.GameFramework.Collections;

namespace BulletSteam.GameFramework.Profile
{
    public class ProfileService : ServiceBase
    {
        private IProfile _profile;
        private readonly Dictionary<int, IProfile> _profiles = new();

        public void RegisterProfile(IProfile profile)
        {
            _profile = profile;
            _profiles.Add(profile.Id, profile);
        }

        public void SetProfile(int id)
        {
            if (_profiles.TryGetValue(id, out IProfile profile))
            {
                _profile = profile;
            }
        }

        public TProfile GetProfile<TProfile>() where TProfile : IProfile
        {
            return (TProfile)_profile;
        }

        public void Load()
        {
            
        }
        
        public void Save()
        {
            
        }

        public T CreateProfile<T>() where T: IProfile, new()
        {
            T profile = new T();
            RegisterProfile(profile);
            return profile;
        }
  
    }

    public interface IProfile
    {
        int Id { get; }


    }
}