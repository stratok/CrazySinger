using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SongDataContainer))]
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _countdown;
    [SerializeField] private AudioClip _start;
    [SerializeField] private AudioClip _lose;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _final;

    private AudioClip _song;
    private AudioSource _audioSource;

    private void Awake()
    {
        _song = GetComponent<SongDataContainer>().SongData.Song;
        _audioSource = GetComponent<AudioSource>();
    }

    public void Stop() => _audioSource.Stop();
    public void Pause() => _audioSource.Pause();
    public void Resume() => _audioSource.Play();

    public void Play(SoundsList sound)
    {
        switch (sound) {
            case SoundsList.Countdown:
                _audioSource.clip = _countdown;
                break;
            case SoundsList.Lose:
                _audioSource.clip = _lose;
                break;
            case SoundsList.Win:
                _audioSource.clip = _win;
                break;
            case SoundsList.Start:
                _audioSource.clip = _start;
                break;
            case SoundsList.Song:
                _audioSource.clip = _song;
                break;
            case SoundsList.Finish:
                _audioSource.clip = _final;
                break;
        }
        
        _audioSource.Play();
    }
}

public enum SoundsList
{
    Countdown,
    Start,
    Lose,
    Win,
    Song,
    Finish
}