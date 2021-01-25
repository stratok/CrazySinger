using System;
using DG.Tweening;
using UnityEngine;

namespace CrazySinger
{
	public class GridController : GameLoopController
	{
		private Vector3 m_StartGridPosition;
		private Transform m_GridTransform;
		private Tween m_MovingTween;
		
		protected override bool IsTimeLoop => false;

		private void Awake()
		{
			m_GridTransform = FindObjectOfType<GridView>().transform;
			m_StartGridPosition = m_GridTransform.position;
		}

		public override void Replay()
		{
			base.Replay();
			m_MovingTween?.Kill();
			m_GridTransform.position = m_StartGridPosition;
		}

		public override void Play()
		{
			base.Play();
			var speed = SongData.FourBeatTime;
			
			m_MovingTween = SongData.UseBaseSpeed ? 
							m_GridTransform.DOMoveX(-SongData.BaseDistance, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental) : 
							m_GridTransform.DOMoveX(-SongData.Distance, speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
		}

		public override void Stop()
		{
			base.Stop();
			m_MovingTween?.Kill();
		}

		protected override void GameLoop() { }
	}
}