using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueComponent : MonoBehaviour, IDataPersistance
{

    string keyName;

    public int StartingEventIndex;
    [SerializeField] public DialogueEvent[] ListOfEvents;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }

    public void LoadData(GameData data)
    {
        keyName = this.gameObject.name + DataManager.instance.Data.CurrentSceneName;

        //Debug.Log("Loading dialogue");

        if (DataManager.instance.Data.DialogueComponentsInGame.ContainsKey(keyName))
        {
            DataManager.instance.Data.DialogueComponentsInGame.TryGetValue(keyName, out StartingEventIndex);
        }
        else
        {
            //Debug.Log($"Making new diction for {this.gameObject.name}, setting value to {StartingEventIndex}");
            DataManager.instance.Data.DialogueComponentsInGame.Add(keyName, StartingEventIndex);
        }
    }

    public void SaveData(ref GameData data)
    {

        keyName = this.gameObject.name + DataManager.instance.Data.CurrentSceneName;

        //Debug.Log("Saving dialogue");

        if (DataManager.instance.Data.DialogueComponentsInGame.ContainsKey(keyName))
        {
            

            DataManager.instance.Data.DialogueComponentsInGame[keyName] = StartingEventIndex;

            //Debug.Log($"Found existing diction for {keyName}, setting value to {DataManager.instance.Data.DialogueComponentsInGame[keyName]}");
        }
        else
        {
            //Debug.Log($"Making new diction for {keyName}, setting value to {StartingEventIndex}");
            DataManager.instance.Data.DialogueComponentsInGame.Add(keyName, StartingEventIndex);
        }
    }

}
