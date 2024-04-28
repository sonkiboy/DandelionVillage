using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class GameData 
{

    public string SpawnPointName;

    public int MaxDandelions;
    public int CurrentDandelions = 0;
    public int GiftedDandelions = 0;

    public Dictionary<GameObject, bool> DandelionsInGame = new Dictionary<GameObject, bool>();

    public GameData()
    {
        MaxDandelions = 8;
        CurrentDandelions = 0;
    }
}
