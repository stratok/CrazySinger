using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SubsController : MonoBehaviour, ISongController
{
    private SongData _songData;
    private Transform _subsPanel;
    private Text _subsText;

    private float _panelShift = -120f;
    private float _shiftDuration = 0.2f;

    private int _stepCount;
    private int _currentStep = 0;
    private float _startTime;
    private Coroutine _activeLoop;

    public void Setup()
    {
        _songData   = FindObjectOfType<SongDataContainer>().SongData;
        _subsPanel  = FindObjectOfType<SubsPanelView>()?.transform;
        _subsText   = FindObjectOfType<SubsTextView>()?.GetComponent<Text>();

        _stepCount = _songData.SongSettings.Length;
        _subsText.text = string.Empty;
        _subsPanel.DOMoveY(_panelShift, 0);
    }

    public void Play()
    {
        _startTime  = Time.time;
        _activeLoop = StartCoroutine(StartLoop());
    }

    public void Stop()
    {
        if (_activeLoop != null)
            StopCoroutine(_activeLoop);

        _currentStep = 0;
    }

    public void Pause()
    {
        if (_activeLoop != null)
            StopCoroutine(_activeLoop);
    }

    public void Resume()
    {
        _activeLoop = StartCoroutine(StartLoop());
    }

    public void Replay()
    {
        if (_activeLoop != null)
            StopCoroutine(_activeLoop);

        _currentStep = 0;
        _activeLoop  = StartCoroutine(StartLoop());
    }

    private IEnumerator StartLoop()
    {
        while (_currentStep < _stepCount)
        {
            float currentTime = (float)Math.Round(Time.time - _startTime, 1);

            if (currentTime == _songData.SongSettings[_currentStep].time)
            {
                _currentStep++;
                ShowNextString();
            }

            yield return null;
        }
    }

    private void ShowNextString()
    {
        _subsPanel.DOMoveY(_panelShift, _shiftDuration).OnComplete(() => 
        {
            _subsText.text = _songData.SongSettings[_currentStep-1].text;
            _subsPanel.DOMoveY(0, _shiftDuration);
        });
    }
    
}

public interface ISongController
{
    void Setup();
    void Play();
    void Stop();
    void Pause();
    void Resume();
    void Replay();
}