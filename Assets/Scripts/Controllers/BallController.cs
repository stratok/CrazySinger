using UnityEngine;

public enum BallCollisionType { None, Bottom, Top }

public class BallController : GameLoopController
{
    [SerializeField]
    private float       _rotateSpeed    = 100;
    private int         _ballSens;
    private Rigidbody2D _ballRigidbody;
    private Transform   _ballTransform;

    private BallCollisionType _ballCollisionType = BallCollisionType.Bottom;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constants.TagObstacles)
            Loss();
        if (collision.gameObject.tag == Constants.TagFinish)
            Win();
        if (collision.gameObject.tag == Constants.TagGroundTop)
            _ballCollisionType = BallCollisionType.Top;
        if (collision.gameObject.tag == Constants.TagGroundBottom)
            _ballCollisionType = BallCollisionType.Bottom;
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
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
    private void Rotate()
    {
        if (_ballCollisionType == BallCollisionType.Bottom)
            _ballTransform.Rotate(Vector3.forward, -_rotateSpeed * Time.deltaTime);
        else if (_ballCollisionType == BallCollisionType.Top)
            _ballTransform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }
    public void Move()
    {
        _ballRigidbody.AddForce(Vector3.up * MicrofonInputManager.MicValue * _ballSens);
    }

    protected override void Setup()
    {
        _ballTransform  = FindObjectOfType<BallView>().transform;
        _ballRigidbody  = _ballTransform.GetComponent<Rigidbody2D>();
        _ballSens       = SaveController.LoadIntFromPrefs(Constants.Sensitivity, 500);
    }
    protected override void GameLoop()
    {
        Move();
        Rotate();
    }
}