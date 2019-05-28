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

    private readonly string _winText = "Безоговорочная победа!";
    private readonly string _lossText = "Вы проиграли, ваш счет: ";
    private readonly string _pauseText = "Пауза";
    private int _currentScore = 0;

    /// <summary>Load Main Menu</summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>Load Calibrate Scene</summary>
    public void LoadCalibrate()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>Load Game Lavel</summary>
    /// <param name="sceneID">Scene ID</param>
    public void LoadLevel(int sceneID)
    {
        if (sceneID > SceneManager.sceneCountInBuildSettings - 1 || sceneID < 0) return;

        SceneManager.LoadScene(sceneID);
    }

    /// <summary>Quit from game</summary>
    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>Show Game Menu</summary>
    /// <param name="state">Game Menu State</param>
    public void ShowGameMenu(GameMenuState state)
    {
        switch (state)
        {
            case GameMenuState.Win:
                _gameMenuPauseButton.SetActive(false);
                _infoText.text = _winText;
                break;
            case GameMenuState.Loss:
                _gameMenuPauseButton.SetActive(false);
                _infoText.text = _lossText + _currentScore.ToString();
                break;
            case GameMenuState.Pause:
                _gameMenuPauseButton.SetActive(true);
                _infoText.text = _pauseText;
                break;
            default:
                break;
        }

        _gameMenu.SetActive(true);
    }

    /// <summary>Hide Game Menu</summary>
    public void HideGameMenu()
    {
        _gameMenu.SetActive(false);
    }

    /// <summary>Show Score</summary>
    /// <param name="score">Score value</param>
    public void ShowScore(int score)
    {
        _currentScore = score;
        _scoreIndicator.text = "Очков: " + _currentScore.ToString();
    }
}