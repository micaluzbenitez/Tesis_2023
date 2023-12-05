using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Toolbox;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("Audio data")]
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer[] audioMixers;

    private const string VolumeKeyName = "Volume";
    private const float LinearToDecibelCoefficient = 20f;
    private const float MinLinearValue = 0.00001f;
    private const float MaxLinearValue = 1f;
    private float sfxVolume = 0;
    private float musicVolume = 0;

    private AudioSource SfxSource => audioSources[(int)MixerType.Sfx];
    private AudioSource MusicSource => audioSources[(int)MixerType.Music];

    private float LinearToDecibel(float linearValue) => LinearToDecibelCoefficient * Mathf.Log10(linearValue);

    #region SINGLETON
    public static AudioManager Instance;
    public static AudioManager Get()
    {
        return Instance;
    }
    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as AudioManager;
    }
    #endregion

    private void Start()
    {
        // Music
        MusicSource musicSource = FindObjectOfType<MusicSource>();

        if (musicSource)
        {
            AudioSource music = musicSource.GetComponent<AudioSource>();

            if (MusicSource.clip != music.clip)
            {
                music.Stop();
                music.clip = MusicSource.clip;
                music.Play();
            }
        }

        // Sliders
        if (sfxSlider) sfxSlider.value = GetVolume(MixerType.Sfx);
        if (musicSlider) musicSlider.value = GetVolume(MixerType.Music);
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

    private float GetVolume(MixerType mixerType)
    {
        float currentMixerValue;
        audioMixers[(int)mixerType].GetFloat(VolumeKeyName, out currentMixerValue);

        // El valor devuelto por GetFloat es en decibelios, así que puedes convertirlo a lineal si es necesario
        float currentVolumeLevel = DecibelToLinear(currentMixerValue);

        return currentVolumeLevel;
    }

    private float DecibelToLinear(float decibels)
    {
        return Mathf.Pow(10.0f, decibels / 20.0f);
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