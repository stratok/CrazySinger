﻿using UnityEngine;

public static class SaveController
{
    public static int LoadIntFromPrefs(string name, int value = 0)
    {
        return PlayerPrefs.GetInt(name, value);
    }

    public static void SaveIntFromPrefs(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }
}