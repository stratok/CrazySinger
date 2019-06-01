using System;
using UnityEngine;

public enum BallCollisionType { None, Bottom, Top }

public class BallController : GameLoopController
{
    private float       _rotateSpeed = 100;
    private int         _ballSens;
    private Rigidbody2D _ballRigidbody;
    private GameController _gameController;

    private BallCollisionType _ballCollisionType = BallCollisionType.Bottom;

    protected override bool IsTimeLoop => false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constants.TagObstacles)
            Lose();
        else if (collision.gameObject.tag == Constants.TagFinish)
            Win();
        else if(collision.gameObject.tag == Constants.TagGroundTop)
            _ballCollisionType = BallCollisionType.Top;
        else if(collision.gameObject.tag == Constants.TagGroundBottom)
            _ballCollisionType = BallCollisionType.Bottom;
    }
    public void OnCollisionExit2D(Collision2D collision) => _ballCollisionType = BallCollisionType.None;

    private void Lose() => _gameController.LoseGame();

    private void Win() => _gameController.WinGame();

    private void Rotate()
    {
        if (_ballCollisionType == BallCollisionType.Bottom)
            transform.Rotate(Vector3.forward, -_rotateSpeed * Time.deltaTime);
        else if (_ballCollisionType == BallCollisionType.Top)
            transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }
    private void Move()
    {
        _ballRigidbody.AddForce(Vector3.up * InputController.MicValue * _ballSens);
    }
    public void ChangeSensitivity(int value) => _ballSens = value;
    public void ResetCollisions() => _ballCollisionType = BallCollisionType.None;

    protected override void Setup()
    {
        _gameController = FindObjectOfType<GameController>();
        _ballRigidbody  = GetComponent<Rigidbody2D>();
        _ballSens       = SaveController.LoadIntFromPrefs(Constants.Sensitivity, 500);
    }
    protected override void GameLoop()
    {
        Move();
        Rotate();
    }

    public override void Play()
    {
        base.Play();

        _ballCollisionType = BallCollisionType.Bottom;
    }
}