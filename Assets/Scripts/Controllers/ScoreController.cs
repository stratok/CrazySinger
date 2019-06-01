public class ScoreController : GameLoopController
{
    private UIController _uIController;

    public int Score { get; private set; }

    protected override bool IsTimeLoop => true;

    protected override void Setup()
    {
        _uIController = FindObjectOfType<UIController>();
        Score = 0;
        Delay = 1;
    }

    protected override void GameLoop()
    {
        Score += 10;
        _uIController.UpdateScore(Score);
    }

    public override void Replay()
    {
        base.Replay();
        Score = 0;
        _uIController.UpdateScore(Score);
    }
}