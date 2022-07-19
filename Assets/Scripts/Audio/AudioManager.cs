using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [Inject] 
    private Sound[] _globalSounds;

    [Inject]
    private AudioMixer _audioMixer;

    private void Awake()
    {
        foreach (Sound sound in _globalSounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
        }
    }

    public SingleSoundOperation withSound(String soundName)
    {
        return new SingleSoundOperation(this, Array.Find(_globalSounds,sound => sound.name == soundName));
    } 

    public SingleSoundOperation withSound(int index)
    {
        return new SingleSoundOperation(this, _globalSounds[index]);
    }

    public SoundGroupOperation withMixerGroupSounds(AudioMixerGroupType groupType)
    {
        return new SoundGroupOperation(this, groupType, _globalSounds);
    }
    
    public class SingleSoundOperation
    {
        private Sound _sound;
        private float _delay;
        private AudioManager _manager;

        internal SingleSoundOperation(AudioManager audioManager, Sound sound)
        {
            _sound = sound;
            _delay = 0;
            _manager = audioManager;
        }

        public SingleSoundOperation withDelay(float delay)
        {
            _delay = delay;
            return this;
        }
        
        public void Play()
        {
            _sound.audioSource.PlayDelayed(_delay);
        }

        public void Stop()
        {
            _sound.audioSource.Stop();
        }

        public void Fade(float duration, float targetVolume)
        {
            _manager.StartCoroutine(StartFade(_sound.audioSource, duration, targetVolume));
        }

        private static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            // TODO: Add other fade in types (non-linear)
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
        }
    }

    public class SoundGroupOperation
    {
        private AudioMixerGroupType _groupType;
        private Sound[] _sounds;
        private AudioManager _manager;
            
        internal SoundGroupOperation(AudioManager audioManager, AudioMixerGroupType groupType, Sound[] sounds)
        {
            _manager = audioManager;
            _groupType = groupType;
            _sounds = Array.FindAll(sounds, 
                sound => convertToAudioMixerGroupType(sound.audioMixerGroup) == groupType);
        }
        
        public void Fade(float duration, float targetVolume)
        {
            
        }
        
        
        private AudioMixerGroupType convertToAudioMixerGroupType(AudioMixerGroup audioMixerGroup)
        {
            switch (audioMixerGroup.name)
            {
                case "Music":
                    return AudioMixerGroupType.MUSIC;
            
                case "UI":
                    return AudioMixerGroupType.UI;
            
                case "SFX":
                    return AudioMixerGroupType.SFX;
            }
            return AudioMixerGroupType.MASTER;
        }
    }

}