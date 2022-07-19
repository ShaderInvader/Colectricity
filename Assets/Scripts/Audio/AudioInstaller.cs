using UnityEngine;
using UnityEngine.Audio;
using Zenject;

[CreateAssetMenu(fileName = "AudioInstaller", menuName = "Installers/AudioInstaller")]
public class AudioInstaller : ScriptableObjectInstaller<AudioInstaller>
{
    public Sound[] sounds;
    public Object globalAudioSourcePrefab;
    public AudioMixer gloablAudioMixer;
    
    public override void InstallBindings()
    {
        Container.Bind<AudioManager>().FromComponentInNewPrefab(globalAudioSourcePrefab).AsSingle().NonLazy();
        Container.Bind<Sound[]>().FromInstance(sounds).AsSingle();
        Container.Bind<AudioMixer>().FromInstance(gloablAudioMixer).AsSingle();
    }
}