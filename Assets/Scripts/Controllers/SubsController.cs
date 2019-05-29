using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SubsController : GameLoopController
{
    private SongData    _songData;
    private Transform   _subsPanel;
    private Text        _subsText;

    private float   _panelShift     = -120f;
    private float   _shiftDuration  = 0.2f;
    private float   _startTime;
    private float   _currentTime;
    private int     _stepCount;
    private int     _currentStep    = 0;

    protected override bool IsTimeLoop => false;

    protected override void Setup()
    {
        _songData   = FindObjectOfType<SongDataContainer>()?.SongData;
        _subsPanel  = FindObjectOfType<SubsPanelView>()?.transform;
        _subsText   = FindObjectOfType<SubsTextView>()?.GetComponent<Text>();

        _stepCount = _songData.SongSettings.Length;
        _subsText.text = string.Empty;
    }

    public override void Play()
    {
        _startTime = Time.time;

        base.Play();
    }

    public override void Stop()
    {
        base.Stop();

        _currentStep = 0;
    }

    public override void Replay()
    {
        base.Replay();

        _currentStep = 0;
    }

    private void ShowNextString()
    {
        _subsPanel.DOMoveY(_panelShift, _shiftDuration).OnComplete(() => 
        {
            _subsText.text = _songData.SongSettings[_currentStep-1].text;
            _subsPanel.DOMoveY(0, _shiftDuration);
        });
    }

    protected override void GameLoop()
    {
        if (_currentStep >= _stepCount) return;
        
        _currentTime = (float)Math.Round(Time.time - _startTime, 1);

        if (_currentTime == _songData.SongSettings[_currentStep].time)
        {
            _currentStep++;
            ShowNextString();
        }
    }
}