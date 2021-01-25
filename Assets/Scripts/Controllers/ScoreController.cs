using System;
using UnityEngine;

namespace CrazySinger
{
	public class ScoreController : GameLoopController
	{
		public int Score { get; private set; }

		public Action<int> OnScoreChanged;

		protected override bool IsTimeLoop => true;

		private void Awake()
		{
			Score = 0;
			Delay = 1;
		}

		protected override void GameLoop()
		{
			Score += 10;
			OnScoreChanged?.Invoke(Score);
		}

		public override void Replay()
		{
			base.Replay();
			Score = 0;
			OnScoreChanged?.Invoke(Score);
		}
	}
}