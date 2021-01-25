using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CrazySinger
{
	public class MainSceneController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private GameObject[] m_SongButtons;
		[SerializeField] private GameObject m_ButtonCalibrate;
		[SerializeField] private GameObject m_ErrorText;
#pragma warning restore 649

		public void Start() => CheckMicrophone();

		public void LoadCalibrate() => SceneManager.LoadScene(1);

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

		private void CheckMicrophone()
		{
			if (Microphone.devices.Length > 0)
				return;
			
			foreach (var button in m_SongButtons)
				button.SetActive(false);
			
			m_ButtonCalibrate.SetActive(false);
			m_ErrorText.SetActive(true);
		}
	}
}