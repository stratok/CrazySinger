using UnityEngine;

namespace CrazySinger
{
	[CreateAssetMenu(fileName = "SongData", menuName = "CustomObjects/SongData")]
	public class SongData : ScriptableObject
	{
		public AudioClip Song;
		public float FinalTime;
		public SongSettings[] SongSettings;
	}

	[System.Serializable]
	public struct SongSettings
	{
		public float time;
		public string text;
	}
}