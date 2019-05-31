using System.Collections;
using UnityEngine;

public class InputController : GameLoopController
{
    // Settings
    private string _device;
    private int _sampleWindow = 128;
    private AudioClip _clipRecord;

    public static float MicValue { get; private set; }
    public static bool IsMenuKeyDown => Input.GetButtonDown("Esc");

    protected override bool IsTimeLoop => false;

    protected override void Setup()
    {
        if (_device == null)
            _device = Microphone.devices[0];
    }

    public override void Play()
    {
        _clipRecord = Microphone.Start(_device, true, 300, 44100);

        base.Play();
    }

    private void StopMicrophone()
    {
        Microphone.End(_device);
    }
    
    private float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
        if (micPosition < 0) return 0;

        _clipRecord.GetData(waveData, micPosition);
        
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        var temp = Mathf.Round(levelMax * 100);
        var micPower = Mathf.Clamp(temp / 100, 0, 1);
        return micPower;
    }

    protected override void GameLoop()
    {
        MicValue = LevelMax();
    }

}