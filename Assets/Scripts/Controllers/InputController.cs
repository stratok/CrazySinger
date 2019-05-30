using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private string _device;
    private AudioClip _clipRecord;
    private int _sampleWindow = 128;

    public static float MicValue { get; private set; }

    public void StartListening()
    {
        InitMic();
    }

    public void StopListening()
    {
        StopMicrophone();
    }

    void InitMic()
    {
        if (_device == null)
            _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 300, 44100);
        StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        while (true)
        {
            MicValue = LevelMax();
            yield return null;
        }
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
        StopAllCoroutines();
    }
    
    float LevelMax()
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
}