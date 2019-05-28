using System.Collections;
using UnityEngine;

public abstract class GameLoopController : MonoBehaviour
{
    protected abstract int Frequency { get; }
    protected bool isLoop;
    protected bool isSetup;

    protected abstract void GameLoop();
    protected abstract void Setup();

    public virtual void Replay() => StartCoroutine(ILoop());
    public virtual void Play() => StartCoroutine(ILoop());
    public virtual void Stop() => isLoop = false;
    public virtual void Pause() => isLoop = false;
    public virtual void Resume() => StartCoroutine(ILoop());

    private IEnumerator ILoop()
    {
        var delay = Frequency / 1000;

        if (!isSetup)
        {
            Setup();
            isSetup = true;
        }
        isLoop = true;

        while (isLoop)
        {
            GameLoop();
            yield return new WaitForSeconds(delay);
        }
    }
}
