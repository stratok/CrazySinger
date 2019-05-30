using UnityEngine;

[RequireComponent(typeof(SongDataContainer))]
[RequireComponent(typeof(AudioSource))]
public class SongController : MonoBehaviour
{
    private AudioClip _audioClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioClip   = GetComponent<SongDataContainer>().SongData.Song;
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
}