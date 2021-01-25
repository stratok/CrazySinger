using System.Collections;
using UnityEngine;

namespace CrazySinger
{
	public abstract class GameLoopController : MonoBehaviour
	{
		protected abstract bool IsTimeLoop { get; }
		protected int Delay = 1;
		protected SongData SongData;
		protected GameController GameController;

		private Coroutine m_GameLoop;

		protected abstract void GameLoop();

		public virtual void Setup(GameController gameController, SongData songData)
		{
			GameController = gameController;
			SongData = songData;
		}

		public virtual void Replay()
		{
			if (m_GameLoop != null)
				StopCoroutine(m_GameLoop);
		}
		public virtual void Play()
		{
			m_GameLoop = StartCoroutine(ILoop());
		}
		public virtual void Stop()
		{
			if (m_GameLoop != null)
				StopCoroutine(m_GameLoop);
		}
		public virtual void Pause() { }
		public virtual void Resume() { }

		private IEnumerator ILoop()
		{
			while (true)
			{
				GameLoop();

				if (IsTimeLoop)
					yield return new WaitForSeconds(Delay);
				else
					yield return null;
			}
		}
	}
}