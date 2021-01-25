using UnityEngine;

namespace CrazySinger
{
	public class InputController : GameLoopController
	{
		private const int SampleWindow = 128;
		
		private string m_Device;
		private AudioClip m_ClipRecord;

		public static float MicValue { get; private set; }
		public static bool IsMenuKeyDown => Input.GetKeyDown(KeyCode.Escape);

		protected override bool IsTimeLoop => false;

		private void Awake()
		{
			if (m_Device == null)
				m_Device = Microphone.devices[0];
		}

		public override void Play()
		{
			m_ClipRecord = Microphone.Start(m_Device, true, 300, 44100);

			base.Play();
		}

		public override void Stop()
		{
			Microphone.End(m_Device);
			base.Stop();
		}

		private float LevelMax()
		{
			float levelMax = 0;
			float[] waveData = new float[SampleWindow];
			int micPosition = Microphone.GetPosition(null) - (SampleWindow + 1);
			if (micPosition < 0) return 0;

			m_ClipRecord.GetData(waveData, micPosition);

			for (int i = 0; i < SampleWindow; i++)
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

		public override void Pause() => Microphone.End(m_Device);
		public override void Resume() => m_ClipRecord = Microphone.Start(m_Device, true, 300, 44100);
	}
}