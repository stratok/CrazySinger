using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class GameController : MonoBehaviour
{
    private Animator _animator;
    
    private UIController UIController;
    private SoundController SoundController;

    private bool _isPause   = false;
    private int  _countdownHesh = Animator.StringToHash("Countdown");

    private GameLoopController[] _controllers;

    private void Awake()
    {
        _animator       = GetComponent<Animator>();

        UIController    = GetComponent<UIController>();
        SoundController = GetComponent<SoundController>();

        _controllers = new GameLoopController[]
        {
            gameObject.AddComponent<ScoreController>(),
            gameObject.AddComponent<SubsController>(),
            gameObject.AddComponent<GridController>(),
            gameObject.AddComponent<InputController>(),
            FindObjectOfType<BallController>(),
        };
    }

    private void Start()
    {
        UIController.HideGameMenu();
        
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
        StartCoroutine(IStartGameCoroutine());
    }

    private IEnumerator IStartGameCoroutine()
    {
        _animator.Play(_countdownHesh);
        yield return null;
        var duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        SoundController.Play(SoundsList.Song);
        Play();
    }

    public void RestartGame()
    {
        Replay();
        
        _isPause = false;
        UIController.HideGameMenu();

        StartGame();
    }

    public void LoseGame()
    {
        Stop();
        
        UIController.ShowGameMenu(GameMenuState.Loss);
        SoundController.Play(SoundsList.Lose);
    }

    public void WinGame()
    {
        Stop();

        SoundController.Play(SoundsList.Win);
        Invoke("ShowMenu", 2);
    }

    private void ShowMenu()
    {
        UIController.ShowGameMenu(GameMenuState.Win);
    }

    private void PlayCountdownSound() => SoundController.Play(SoundsList.Countdown);
    private void PlayStartSound() => SoundController.Play(SoundsList.Start);

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        if (_isPause)
        {
            Time.timeScale = 1;
            UIController.HideGameMenu();
            SoundController.Resume();
            Resume();
        }
        else
        {
            Time.timeScale = 0;
            UIController.ShowGameMenu(GameMenuState.Pause);
            SoundController.Pause();
            Pause();
        }

        _isPause = !_isPause;
    }

    private void Play()
    {
        for (int i = 0; i < _controllers.Length; i++)
            _controllers[i].Play();
    }
    private void Stop()
    {
        for (int i = 0; i < _controllers.Length; i++)
            _controllers[i].Stop();
    }
    private void Replay()
    {
        for (int i = 0; i < _controllers.Length; i++)
            _controllers[i].Replay();
    }
    private void Pause()
    {
        for (int i = 0; i < _controllers.Length; i++)
            _controllers[i].Pause();
    }
    private void Resume()
    {
        for (int i = 0; i < _controllers.Length; i++)
            _controllers[i].Resume();
    }
}