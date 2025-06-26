using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public class AudioSystem : MonoBehaviour, IAudioSystem
    {
        public void PlayAudio(AudioClip clip, Vector2 position, float volume = 1f)
        {

            Debug.Log($"Playing audio: {clip.name} at {position} with volume {volume}");
        }
    }
}