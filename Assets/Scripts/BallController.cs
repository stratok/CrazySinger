using System.Collections;
using UnityEngine;

public enum BallCollisionType
{
    None,
    Bottom,
    Top
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour, ISongController
{
    [SerializeField] private float _rotateSpeed = 100;

    private int _ballSens = 500;

    private Collider2D  _collider;
    private Rigidbody2D _rigidbody;
    private Coroutine   _activeLoop;

    private BallCollisionType _ballCollisionType;

    private bool _isCalibrate;

    public void StartCalibrate()
    {
        _isCalibrate = true;
    }

    public void MoveUP()
    {
        _rigidbody.AddForce(Vector3.up * MicrofonInputManager.MicValue * _ballSens);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacles")
            Loss();
        if (collision.gameObject.tag == "Finish")
            Win();
        if (collision.gameObject.tag == "TopGround")
            _ballCollisionType = BallCollisionType.Top;
        if (collision.gameObject.tag == "BottomGround")
            _ballCollisionType = BallCollisionType.Bottom;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TopGround" || collision.gameObject.tag == "BottomGround")
            _ballCollisionType = BallCollisionType.None;
    }

    public void Loss()
    {
        GameController.Instance.StopGame();
    }

    public void Win()
    {
        GameController.Instance.WinGame();
    }

    private void RotateBall()
    {
        if (_ballCollisionType == BallCollisionType.Bottom)
            transform.Rotate(Vector3.forward, -_rotateSpeed * Time.deltaTime);
        else if (_ballCollisionType == BallCollisionType.Top)
            transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }

    public void Setup()
    {
        _collider   = GetComponent<Collider2D>();
        _rigidbody  = GetComponent<Rigidbody2D>();
        _ballSens   = SaveController.LoadIntFromPrefs("MicSensitivity");
    }
    private IEnumerator GameLoop()
    {
        while (true)
        {
            MoveUP();
            RotateBall();

            yield return null;
        }
    }

    public void Play()
    {
        _activeLoop = StartCoroutine(GameLoop());
    }

    public void Stop()
    {
        if (_activeLoop != null)
            StopCoroutine(_activeLoop);
    }

    public void Pause()
    {
        if (_activeLoop != null)
            StopCoroutine(_activeLoop);
    }

    public void Resume()
    {
        _activeLoop = StartCoroutine(GameLoop());
    }

    public void Replay()
    {
        throw new System.NotImplementedException();
    }
}
