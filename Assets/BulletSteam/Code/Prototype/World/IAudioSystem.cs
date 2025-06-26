using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public interface IAudioSystem
    {
        void PlayAudio(AudioClip clip, Vector2 position, float volume = 1f);
    }
}