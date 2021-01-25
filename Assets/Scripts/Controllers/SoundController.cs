using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrazySinger
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private AudioSource m_AudioSourceMain;
		[SerializeField] private AudioSource m_AudioSourceUI;
		[SerializeField] private AudioClip m_Countdown;
		[SerializeField] private AudioClip m_Start;
		[SerializeField] private AudioClip m_Lose;
		[SerializeField] private AudioClip m_Win;
		[SerializeField] private AudioClip m_Finish;
		[SerializeField] private AudioClip m_Click;
#pragma warning restore 649

		private Dictionary<GameSound, AudioClip> m_SongsDictionary;

		public static SoundController I { get; private set; }

		private void Awake()
		{
			if (I == null) 
			{
				I = this;
			} 
			else if(I == this)
			{
				Destroy(gameObject);
			}
			
			DontDestroyOnLoad(gameObject);
			
			m_SongsDictionary = new Dictionary<GameSound, AudioClip>()
			{
				{GameSound.Countdown, m_Countdown},
				{GameSound.Lose, m_Lose},
				{GameSound.Start, m_Start},
				{GameSound.Win, m_Win},
				{GameSound.Finish, m_Finish},
				{GameSound.Click, m_Click},
				{GameSound.Song, null}
			};
		}

		public void Stop() => m_AudioSourceMain.Stop();
		public void Pause() => m_AudioSourceMain.Pause();
		public void Resume() => m_AudioSourceMain.Play();
		public void Setup(AudioClip song) => m_SongsDictionary[GameSound.Song] = song;

		public void Play(GameSound sound, SoundChannel channel)
		{
			var clip = m_SongsDictionary[sound];
			
			switch (channel)
			{
				case SoundChannel.UI:
					m_AudioSourceUI.clip = clip;
					m_AudioSourceUI.Play();
					break;
				case SoundChannel.Main:
					m_AudioSourceMain.clip = clip;
					m_AudioSourceMain.Play();
					break;
			}
		}
	}

	public enum SoundChannel
	{
		UI,
		Main
	}
}