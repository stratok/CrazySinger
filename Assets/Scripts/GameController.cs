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

    private bool _isRun     = false;
    private bool _isPause   = false;
    private Vector3 _startGridPosition;
    private int _score = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _ballController = FindObjectOfType<BallController>();
        _microfonInput = FindObjectOfType<MicrofonInputManager>();
        _uIController = FindObjectOfType<UIController>();

        SubsController = gameObject.AddComponent<SubsController>();
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
        _score = 0;
        _animator.Play("Countdown");
        yield return null;
        var duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        _isRun = true;
        _microfonInput.StartListening();
        PlaySound(_song);
        _ballController.Play();

        SubsController.Play();
        StartCoroutine(IIncreaseScore());
    }

    private IEnumerator IIncreaseScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _score += 10;
            _uIController.ShowScore(_score);
        }
    }

    public void RestartGame()
    {
        SubsController.Replay();

        _isRun = false;
        _microfonInput.StopListening();
        _ballController.Stop();
        _isPause = false;
        _uIController.HideGameMenu();
        _score = 0;
        StopAllCoroutines();
        StartGame();
    }

    public void StopGame()
    {
        if (!_isRun) return;

        SubsController.Stop();

        _microfonInput.StopListening();
        _isRun = false;
        _uIController.ShowGameMenu(GameMenuState.Loss);
        PlaySound(_soundLoss);
        StopAllCoroutines();
    }

    public void WinGame()
    {
        _microfonInput.StopListening();

        SubsController.Stop();

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
            SubsController.Resume();
        }
        else
        {
            _isPause = true;
            Time.timeScale = 0;
            _uIController.ShowGameMenu(GameMenuState.Pause);
            _audioSource.Pause();

            SubsController.Pause();
        }
    }
}