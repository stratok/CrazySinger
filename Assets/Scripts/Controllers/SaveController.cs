using UnityEngine;

namespace CrazySinger
{
	public static class SaveController
	{
		public static int LoadIntFromPrefs(string name, int value = 0)
		{
			return PlayerPrefs.GetInt(name, value);
		}

		public static void SaveIntToPrefs(string name, int value)
		{
			PlayerPrefs.SetInt(name, value);
			PlayerPrefs.Save();
		}
	}
}