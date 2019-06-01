using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private GameObject _buttonStart;
    [SerializeField] private GameObject _buttonCalibrate;
    [SerializeField] private GameObject _errorText;

    public void Start() => CheckMicrophon();

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

    private void CheckMicrophon()
    {
        if (Microphone.devices.Length == 0)
        {
            _buttonStart.SetActive(false);
            _buttonCalibrate.SetActive(false);
            _errorText.SetActive(true);
        }
    }
}