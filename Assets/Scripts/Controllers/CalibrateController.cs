using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalibrateController : MonoBehaviour
{
    private InputController _microfonInput;
    private BallController[] _ballControllers;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private Text   _sliderValue;

    private void Start()
    {
        _microfonInput   = GetComponent<InputController>();
        _ballControllers = FindObjectsOfType<BallController>();

        var sensitivity = SaveController.LoadIntFromPrefs(Constants.Sensitivity, 500);
        _slider.value = sensitivity;

        _microfonInput.Play();

        for (int i = 0; i < _ballControllers.Length; i++)
        {
            _ballControllers[i].Play();
            _ballControllers[i].ResetCollisions();
        }
    }

    public void ChangeSensitivity(float value)
    {
        _sliderValue.text = _slider.value.ToString();

        for (int i = 0; i < _ballControllers.Length; i++)
            _ballControllers[i].ChangeSensitivity((int)_slider.value);
    }

    public void SaveSettings()
    {
        SaveController.SaveIntToPrefs(Constants.Sensitivity, (int)_slider.value);
        SceneManager.LoadScene(0);
    }
}
