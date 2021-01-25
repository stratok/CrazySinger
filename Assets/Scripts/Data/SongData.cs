using System;
using UnityEngine;

namespace CrazySinger
{
	[CreateAssetMenu(fileName = "SongData", menuName = "CustomObjects/SongData")]
	public class SongData : ScriptableObject
	{
		public AudioClip Song;
		public int BPM = 120;
		public int Distance = 8;
		public int BaseDistance = 2;
		public bool UseBaseSpeed;
		public SongSettings[] SongSettings;

		public float OneBeatTime => ((float) 60 / BPM);
		public float FourBeatTime => OneBeatTime * 4;
		public float BallSpeed => FourBeatTime * 100;
	}

	[Serializable]
	public struct SongSettings
	{
		public string text;
		public float time;
	}
}