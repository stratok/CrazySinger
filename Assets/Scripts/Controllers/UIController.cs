using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameMenuState { Win, Loss, Pause }

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _gameMenuPauseButton;
    [SerializeField] private Text _scoreIndicator;
    [SerializeField] private Text _infoText;
    
    private int _currentScore = 0;
    
    public void LoadMainMenu()  => SceneManager.LoadScene(0);
    public void LoadCalibrate() => SceneManager.LoadScene(1);
    public void HideGameMenu()  => _gameMenu.SetActive(false);
    
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
                _gameMenuPauseButton.SetActive(false);
                _infoText.text = Constants.WinText;
                break;
            case GameMenuState.Loss:
                _gameMenuPauseButton.SetActive(false);
                _infoText.text = Constants.LossText + _currentScore.ToString();
                break;
            case GameMenuState.Pause:
                _gameMenuPauseButton.SetActive(true);
                _infoText.text = Constants.PauseText;
                break;
            default:
                break;
        }

        _gameMenu.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        _currentScore = score;
        _scoreIndicator.text = $"Очков: {_currentScore.ToString()}";
    }
}