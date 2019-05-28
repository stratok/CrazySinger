using UnityEngine;

public static class SaveController
{
    public static int LoadIntFromPrefs(string name)
    {
        return PlayerPrefs.GetInt(name);
    }

    public static void SaveIntFromPrefs(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }
}