using System.Collections;
using UnityEngine;

public abstract class GameLoopController : MonoBehaviour
{
    protected bool isSetup;
    private Coroutine _gameLoop;

    protected abstract bool IsTimeLoop { get; }
    protected int Delay = 1;

    protected abstract void GameLoop();
    protected abstract void Setup();

    public virtual void Replay()
    {
        if(_gameLoop != null)
            StopCoroutine(_gameLoop);

        _gameLoop = StartCoroutine(ILoop());
    }
    public virtual void Play()
    {
        if (!isSetup)
        {
            Setup();
            isSetup = true;
        }
        _gameLoop = StartCoroutine(ILoop());
    }
    public virtual void Stop()
    {
        if (_gameLoop != null)
            StopCoroutine(_gameLoop);
    }
    public virtual void Pause()
    {
        if (_gameLoop != null)
            StopCoroutine(_gameLoop);
    }
    public virtual void Resume() => _gameLoop = StartCoroutine(ILoop());

    private IEnumerator ILoop()
    {
        while (true)
        {
            GameLoop();

            if (IsTimeLoop)
                yield return new WaitForSeconds(Delay);
            else
                yield return null;
        }
    }
}
