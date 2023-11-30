using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Toolbox;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [Header("Audio data")]
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer[] audioMixers;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic = null;

    private const string VolumeKeyName = "Volume";
    private const float LinearToDecibelCoefficient = 20f;
    private const float MinLinearValue = 0.00001f;
    private const float MaxLinearValue = 1f;
    private float sfxVolume = 0;
    private float musicVolume = 0;

    private AudioSource SfxSource => audioSources[(int)MixerType.Sfx];
    private AudioSource MusicSource => audioSources[(int)MixerType.Music];

    private float LinearToDecibel(float linearValue) => LinearToDecibelCoefficient * Mathf.Log10(linearValue);

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlaySfx(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void SwitchPauseState()
    {
        if (MusicSource.isPlaying)
            MusicSource.Pause();
        else
            MusicSource.Play();
    }

    public void StopMusic()
    {
        if (MusicSource.isPlaying)
            MusicSource.Stop();
    }

    public void SetSFXVolume(float volumeLevel)
    {
        SetVolume(MixerType.Sfx, sfxSlider.value);
    }

    public void SetMusicVolume(float volumeLevel)
    {
        SetVolume(MixerType.Music, musicSlider.value);
    }

    private void SetVolume(MixerType mixerType, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, MinLinearValue, MaxLinearValue);

        float desiredMixerDecibels = LinearToDecibel(volumeLevel);

        audioMixers[(int)mixerType].SetFloat(VolumeKeyName, desiredMixerDecibels);
    }

    public void MuteSFX()
    {
        sfxVolume = sfxSlider.value;
        sfxSlider.value = MinLinearValue;
        SetSFXVolume(MinLinearValue);
    }

    public void MuteMusic()
    {
        musicVolume = musicSlider.value;
        musicSlider.value = MinLinearValue;
        SetMusicVolume(MinLinearValue);
    }

    public void UnmuteSFX()
    {
        sfxSlider.value = sfxVolume;
        SetSFXVolume(sfxVolume);
    }

    public void UnmuteMusic()
    {
        musicSlider.value = musicVolume;
        SetMusicVolume(musicVolume);
    }
}