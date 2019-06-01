using UnityEngine;

public class GridController : GameLoopController
{
    private float _gridSpeed = 2f;
    private Vector3 _startGridPosition;
    private Transform _gridTransform;

    protected override bool IsTimeLoop => false;
    
    protected override void Setup()
    {
        _gridTransform = FindObjectOfType<GridView>().transform;
        _startGridPosition = _gridTransform.position;
    }

    public override void Replay()
    {
        base.Replay();

        _gridTransform.position = _startGridPosition;
    }

    protected override void GameLoop()
    {
        _gridTransform.position = _gridTransform.position + Vector3.left * Time.deltaTime * _gridSpeed;
    }
}