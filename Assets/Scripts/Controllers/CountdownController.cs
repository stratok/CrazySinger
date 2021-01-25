using System;
using DG.Tweening;
using UnityEngine;

namespace CrazySinger
{
	public class CountdownController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private RectTransform m_Three;
		[SerializeField] private RectTransform m_Two;
		[SerializeField] private RectTransform m_One;
		[SerializeField] private RectTransform m_Go;
#pragma warning restore 649

		public void StartCountdown(Action onComplete, SongData songData)
		{
			var bitTime = songData.OneBeatTime;
			var halfBeatTime = bitTime * 0.5f;

			DOTween.Sequence()
				.Append(m_Three.DOScale(1, halfBeatTime))
				.AppendCallback(()=>SoundController.I.Play(GameSound.Countdown, SoundChannel.UI))
				.AppendInterval(halfBeatTime)
				.Append(m_Two.DOScale(1, halfBeatTime))
				.Join(m_Three.DOScale(0, 0))
				.AppendCallback(()=>SoundController.I.Play(GameSound.Countdown, SoundChannel.UI))
				.AppendInterval(halfBeatTime)
				.Append(m_One.DOScale(1, halfBeatTime))
				.Join(m_Two.DOScale(0, 0))
				.AppendCallback(()=>SoundController.I.Play(GameSound.Countdown, SoundChannel.UI))
				.AppendInterval(halfBeatTime)
				.Append(m_Go.DOScale(1, halfBeatTime))
				.Join(m_One.DOScale(0, 0))
				.AppendCallback(()=>SoundController.I.Play(GameSound.Start, SoundChannel.UI))
				.AppendInterval(bitTime)
				.Append(m_Go.DOScale(0, 0))
				.AppendCallback(onComplete.Invoke);
		}
	}
}