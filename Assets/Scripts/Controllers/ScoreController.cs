public class ScoreController : GameLoopController
{
    private UIController _uIController;

    public int Score { get; private set; }

    protected override void GameLoop()
    {
        Score += 10;
        _uIController.ShowScore(Score);
    }

    protected override void Setup()
    {
        _uIController = GetComponent<UIController>();
        Score = 0;
    }
}