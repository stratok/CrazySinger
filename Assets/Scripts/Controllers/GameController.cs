using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameController>();

            return _instance;
        }
    }

    [Header("Sounds")]
    [SerializeField] private AudioClip _soundCountdown;
    [SerializeField] private AudioClip _soundStart;
    [SerializeField] private AudioClip _soundLoss;
    [SerializeField] private AudioClip _soundWin;
    [SerializeField] private Transform _gridTransform;

    [Tooltip("Скорость движения полотна")]
    [SerializeField] private float _gridSpeed;

    // Components
    private Animator _animator;
    private AudioSource _audioSource;
    
    private UIController       UIController;
    private InputController    InputController;
    private SongController     SongController;

    private GameLoopController SubsController;
    private GameLoopController BallController;
    private GameLoopController ScoreController;

    private bool _isRun     = false;
    private bool _isPause   = false;
    private Vector3 _startGridPosition;

    private void Awake()
    {
        _animator       = GetComponent<Animator>();
        _audioSource    = GetComponent<AudioSource>();

        UIController    = FindObjectOfType<UIController>();
        InputController = FindObjectOfType<InputController>();
        SubsController  = FindObjectOfType<SubsController>();
        SongController  = FindObjectOfType<SongController>();

        ScoreController = gameObject.AddComponent<ScoreController>();
        BallController  = gameObject.AddComponent<BallController>();
    }

    private void Start()
    {
        _startGridPosition = _gridTransform.position;

        UIController.HideGameMenu();
        
        StartGame();
    }

    private void Update()
    {
        _gridTransform.position = _gridTransform.position + Vector3.left * Time.deltaTime * _gridSpeed;

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void StartGame()
    {
        _gridTransform.position = _startGridPosition;

        UIController.HideGameMenu();

        Time.timeScale = 1;

        StartCoroutine(IStartGameCoroutine());
    }

    private IEnumerator IStartGameCoroutine()
    {
        _animator.Play("Countdown");
        yield return null;
        var duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        InputController.StartListening();

        SongController.Play();
        ScoreController.Play();
        BallController.Play();
        SubsController.Play();
    }

    public void RestartGame()
    {
        ScoreController.Replay();
        SubsController.Replay();
        BallController.Replay();

        _isRun = false;
        InputController.StopListening();
        _isPause = false;
        UIController.HideGameMenu();

        StartGame();
    }

    public void StopGame()
    {
        ScoreController.Stop();
        SubsController.Stop();
        BallController.Stop();

        InputController.StopListening();
        _isRun = false;
        UIController.ShowGameMenu(GameMenuState.Loss);
        PlaySound(_soundLoss);
    }

    public void WinGame()
    {
        InputController.StopListening();

        ScoreController.Stop();
        SubsController.Stop();
        BallController.Stop();

        _isRun = false;
        PlaySound(_soundWin);
        StopGame();
        Invoke("ShowMenu", 2);
    }

    private void ShowMenu()
    {
        UIController.ShowGameMenu(GameMenuState.Win);
    }

    private void PlayCountdownSound()
    {
        PlaySound(_soundCountdown);
    }

    private void PlayStartSound()
    {
        PlaySound(_soundStart);
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        if (_isPause)
        {
            _isPause = false;
            Time.timeScale = 1;
            UIController.HideGameMenu();
            _audioSource.Play();

            ScoreController.Resume();
            SubsController.Resume();
            BallController.Resume();
        }
        else
        {
            _isPause = true;
            Time.timeScale = 0;
            UIController.ShowGameMenu(GameMenuState.Pause);
            _audioSource.Pause();

            ScoreController.Pause();
            SubsController.Pause();
            BallController.Pause();
        }
    }
}