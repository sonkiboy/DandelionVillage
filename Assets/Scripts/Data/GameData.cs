using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public string CurrentSceneName;
    public string SpawnPointName;

    public int MaxDandelions;
    public int CurrentDandelions = 0;
    public int GiftedDandelions = 0;

    public Dictionary<string, bool> DandelionsInGame = new Dictionary<string, bool>();
    public Dictionary<string,int> DialogueComponentsInGame = new Dictionary<string,int>();

    public GameData()
    {
        MaxDandelions = 8;
        CurrentDandelions = 0;
        GiftedDandelions = 0;
    }
}
