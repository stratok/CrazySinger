using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CrazySinger
{
	public class UIController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private GameObject m_GameMenu;
		[SerializeField] private GameObject m_PauseButton;
		[SerializeField] private TextMeshProUGUI m_ScoreIndicator;
		[SerializeField] private TextMeshProUGUI m_InfoText;
		[SerializeField] private ScoreController m_ScoreController;
#pragma warning restore 649

		private int m_CurrentScore;

		private void Awake()
		{
			m_ScoreController.OnScoreChanged += UpdateScore;
		}

		private void OnDestroy()
		{
			m_ScoreController.OnScoreChanged -= UpdateScore;
		}

		[UsedImplicitly]
		public void LoadMainMenu() => SceneManager.LoadScene(0);
		public void LoadCalibrate() => SceneManager.LoadScene(1);
		public void HideGameMenu() => m_GameMenu.SetActive(false);

		public void LoadLevel(int sceneID)
		{
			if (sceneID > SceneManager.sceneCountInBuildSettings - 1 || sceneID < 0) return;

			SceneManager.LoadScene(sceneID);
		}

		public void Quit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
		}

		public void ShowGameMenu(GameMenuState state)
		{
			switch (state)
			{
				case GameMenuState.Win:
					m_PauseButton.SetActive(false);
					m_InfoText.text = Constants.WinText;
					break;
				case GameMenuState.Loss:
					m_PauseButton.SetActive(false);
					m_InfoText.text = Constants.LossText + m_CurrentScore.ToString();
					break;
				case GameMenuState.Pause:
					m_PauseButton.SetActive(true);
					m_InfoText.text = Constants.PauseText;
					break;
				default:
					throw new NotImplementedException();
			}

			m_GameMenu.SetActive(true);
		}

		public void UpdateScore(int score)
		{
			m_CurrentScore = score;
			m_ScoreIndicator.text = $"Очков: {m_CurrentScore.ToString()}";
		}
	}
}