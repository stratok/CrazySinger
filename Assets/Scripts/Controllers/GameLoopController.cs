using System.Collections;
using UnityEngine;

public abstract class GameLoopController : MonoBehaviour
{
    protected bool isLoop;
    protected bool isSetup;

    protected abstract bool IsTimeLoop { get; }
    protected int Delay = 1;

    protected abstract void GameLoop();
    protected abstract void Setup();

    public virtual void Replay() => StartCoroutine(ILoop());
    public virtual void Play()   => StartCoroutine(ILoop());
    public virtual void Stop()   => isLoop = false;
    public virtual void Pause()  => isLoop = false;
    public virtual void Resume() => StartCoroutine(ILoop());

    private IEnumerator ILoop()
    {
        if (!isSetup)
        {
            Setup();
            isSetup = true;
        }
        isLoop = true;

        while (isLoop)
        {
            GameLoop();

            if (IsTimeLoop)
                yield return new WaitForSeconds(Delay);
            else
                yield return null;
        }
    }
}
