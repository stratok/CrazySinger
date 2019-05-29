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
                _instance = (GameController)FindObjectOfType<GameController>();

            return _instance;
        }
    }

    [Header("Sounds")]
    [SerializeField] private AudioClip _soundCountdown;
    [SerializeField] private AudioClip _soundStart;
    [SerializeField] private AudioClip _soundLoss;
    [SerializeField] private AudioClip _soundWin;
    [SerializeField] private AudioClip _song;
    
    [SerializeField] private Transform _gridTransform;


    [Tooltip("Скорость движения полотна")]
    [SerializeField] private float _gridSpeed;

    // Components
    private Animator _animator;
    private AudioSource _audioSource;
    private BallController _ballController;
    private UIController _uIController;
    private MicrofonInputManager _microfonInput;

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
        _microfonInput  = FindObjectOfType<MicrofonInputManager>();
        _uIController   = FindObjectOfType<UIController>();

        ScoreController = gameObject.AddComponent<ScoreController>();
        SubsController  = gameObject.AddComponent<SubsController>();
        BallController  = gameObject.AddComponent<BallController>();
    }

    private void Start()
    {
        _startGridPosition = _gridTransform.position;

        _uIController.HideGameMenu();
        
        StartGame();
    }

    private void Update()
    {
        if (!_isRun) return;

        _gridTransform.position = _gridTransform.position + Vector3.left * Time.deltaTime * _gridSpeed;

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void StartGame()
    {
        _gridTransform.position = _startGridPosition;

        _uIController.HideGameMenu();

        Time.timeScale = 1;

        StartCoroutine(IStartGameCoroutine());
    }

    private IEnumerator IStartGameCoroutine()
    {
        _animator.Play("Countdown");
        yield return null;
        var duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        _isRun = true;
        _microfonInput.StartListening();
        PlaySound(_song);

        ScoreController.Play();
        BallController.Play();
        SubsController.Play();
    }

    public void RestartGame()
    {
        SubsController.Replay();
        BallController.Replay();

        _isRun = false;
        _microfonInput.StopListening();
        _isPause = false;
        _uIController.HideGameMenu();

        StopAllCoroutines();
        StartGame();
    }

    public void StopGame()
    {
        if (!_isRun) return;

        ScoreController.Stop();
        SubsController.Stop();
        BallController.Stop();

        _microfonInput.StopListening();
        _isRun = false;
        _uIController.ShowGameMenu(GameMenuState.Loss);
        PlaySound(_soundLoss);
        StopAllCoroutines();
    }

    public void WinGame()
    {
        _microfonInput.StopListening();

        ScoreController.Stop();
        SubsController.Stop();
        BallController.Stop();

        _isRun = false;
        PlaySound(_soundWin);
        StopAllCoroutines();
        Invoke("ShowMenu", 2);
    }

    private void ShowMenu()
    {
        _uIController.ShowGameMenu(GameMenuState.Win);
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
            _uIController.HideGameMenu();
            _audioSource.Play();

            ScoreController.Resume();
            SubsController.Resume();
            BallController.Resume();
        }
        else
        {
            _isPause = true;
            Time.timeScale = 0;
            _uIController.ShowGameMenu(GameMenuState.Pause);
            _audioSource.Pause();

            ScoreController.Pause();
            SubsController.Pause();
            BallController.Pause();
        }
    }
}