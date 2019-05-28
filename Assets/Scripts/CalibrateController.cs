using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalibrateController : MonoBehaviour
{
    private MicrofonInputManager _microfonInput;
    private BallController[] _ballControllers;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private Text   _sliderValue;

    //private void Start()
    //{
    //    _microfonInput  = FindObjectOfType<MicrofonInputManager>();
    //    _ballControllers = FindObjectsOfType<BallController>();
    //    SaveController.LoadIntFromPrefs();

    //    _slider.value = SaveController.BallSensitivity;
    //    _microfonInput.StartListening();
    //    for (int i = 0; i < _ballControllers.Length; i++)
    //    {
    //        _ballControllers[i].StartCalibrate();
    //    }
    //}

    //public void ChangeSensitivity(float value)
    //{
    //    SaveController.BallSensitivity = value;
    //    _sliderValue.text = SaveController.BallSensitivity.ToString();
    //}
    
    //public void SaveSettings()
    //{
    //    SaveController.SaveFloatFromPrefs();
    //    SceneManager.LoadScene(0);
    //}
}
