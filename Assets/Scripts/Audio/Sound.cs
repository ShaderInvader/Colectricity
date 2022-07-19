﻿using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public String name;
    
    public AudioClip audioClip;

    public AudioMixerGroup AudioMixerGroup;
    
    [Range(0, 1)]
    public float volume;
    
    [Range(0.1f, 3)]
    public float pitch;
}