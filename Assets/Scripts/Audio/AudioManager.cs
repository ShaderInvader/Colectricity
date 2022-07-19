using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [Inject] 
    private Sound[] _globalSounds;

    [Inject]
    private AudioMixer _audioMixer;
    
}