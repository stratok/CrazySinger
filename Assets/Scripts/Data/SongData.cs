using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "CustomObjects/SongData")]
public class SongData : ScriptableObject
{
    public SongSettings[] SongSettings;
}

[System.Serializable]
public struct SongSettings
{
    public float time;
    public string text;
}