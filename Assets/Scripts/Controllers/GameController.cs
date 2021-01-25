using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazySinger
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private BallController m_BallController;
		[SerializeField] private CountdownController m_CountdownController;
		[SerializeField] private UIController m_UIController;
		[SerializeField] private SongData m_SongData;
		
		private bool m_IsPause;
		private GameLoopController[] m_Controllers;

		private void Awake()
		{
			m_Controllers = new GameLoopController[]
			{
				GetComponent<ScoreController>(),
				GetComponent<SubsController>(),
				GetComponent<GridController>(),
				GetComponent<InputController>(),
				m_BallController,
			};
		}

		private void Start()
		{
			Setup();
			m_UIController.HideGameMenu();
			StartGame();
		}

		private void Update()
		{
			if (InputController.IsMenuKeyDown)
				PauseGame();
		}

		public void StartGame()
		{
			Time.timeScale = 1;
			m_CountdownController.StartCountdown(() =>
			{
				SoundController.I.Play(GameSound.Song, SoundChannel.Main);
				Play();
			}, m_SongData);
		}

		public void RestartGame()
		{
			SoundController.I.Stop();
			Replay();

			m_IsPause = false;
			m_UIController.HideGameMenu();

			StartGame();
		}

		public void LoseGame()
		{
			Stop();

			SoundController.I.Stop();
			SoundController.I.Play(GameSound.Lose, SoundChannel.UI);
			m_UIController.ShowGameMenu(GameMenuState.Loss);
		}

		public void WinGame()
		{
			Stop();
			SoundController.I.Stop();
			SoundController.I.Play(GameSound.Win, SoundChannel.UI);
			Invoke(nameof(ShowMenu), 2);
		}

		private void ShowMenu()
		{
			m_UIController.ShowGameMenu(GameMenuState.Win);
		}

		public void Exit()
		{
			Time.timeScale = 1;
			SoundController.I.Stop();
			SceneManager.LoadScene(0);
		}

		public void PauseGame()
		{
			if (m_IsPause)
			{
				Time.timeScale = 1;
				m_UIController.HideGameMenu();
				SoundController.I.Resume();
				Resume();
			}
			else
			{
				Time.timeScale = 0;
				m_UIController.ShowGameMenu(GameMenuState.Pause);
				SoundController.I.Pause();
				Pause();
			}

			m_IsPause = !m_IsPause;
		}

		private void Setup()
		{
			SoundController.I.Setup(m_SongData.Song);
			
			for (int i = 0; i < m_Controllers.Length; i++)
				m_Controllers[i].Setup(this, m_SongData);
		}

		private void Play()
		{
			for (int i = 0; i < m_Controllers.Length; i++)
				m_Controllers[i].Play();
		}
		
		private void Stop()
		{
			for (int i = 0; i < m_Controllers.Length; i++)
				m_Controllers[i].Stop();
		}
		
		private void Replay()
		{
			for (int i = 0; i < m_Controllers.Length; i++)
				m_Controllers[i].Replay();
		}
		
		private void Pause()
		{
			for (int i = 0; i < m_Controllers.Length; i++)
				m_Controllers[i].Pause();
		}
		
		private void Resume()
		{
			for (int i = 0; i < m_Controllers.Length; i++)
				m_Controllers[i].Resume();
		}
	}
}