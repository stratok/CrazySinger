using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CrazySinger
{
	public class CalibrateController : MonoBehaviour
	{
		private InputController m_MicrophoneInput;
		private SimpleBallController[] m_BallControllers;

#pragma warning disable 649
		[SerializeField] private Slider _slider;
		[SerializeField] private Text _sliderValue;
#pragma warning restore 649

		private void Start()
		{
			m_MicrophoneInput = GetComponent<InputController>();
			m_BallControllers = FindObjectsOfType<SimpleBallController>();

			var sensitivity = SaveController.LoadIntFromPrefs(Constants.Sensitivity, 500);
			_slider.value = sensitivity;
	
			m_MicrophoneInput.Play();
		}

		public void ChangeSensitivity(float value)
		{
			_sliderValue.text = _slider.value.ToString();

			for (int i = 0; i < m_BallControllers.Length; i++)
				m_BallControllers[i].ChangeSensitivity((int)_slider.value);
		}

		public void SaveSettings()
		{
			SaveController.SaveIntToPrefs(Constants.Sensitivity, (int)_slider.value);
			SceneManager.LoadScene(0);
		}
	}
}