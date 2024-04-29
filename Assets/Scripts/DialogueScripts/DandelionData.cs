using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionData : MonoBehaviour, IDataPersistance
{
    string dataName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameData data)
    {


        dataName = this.gameObject.name + DataManager.instance.Data.CurrentSceneName;

        Debug.Log($"{dataName} load");

        if (DataManager.instance.Data.DandelionsInGame.ContainsKey(dataName))
        {
            Debug.Log($"{dataName} found, value is {DataManager.instance.Data.DandelionsInGame[dataName]}");

            if (DataManager.instance.Data.DandelionsInGame[dataName] == false)
            {
                Debug.Log($"{dataName} already picked up, deleting");
                Destroy(this.gameObject);
            }
        }
        else
        {
            DataManager.instance.Data.DandelionsInGame.Add(dataName, true);
        }

    }


    public void SaveData(ref GameData data)
    {
        dataName = this.gameObject.name + DataManager.instance.Data.CurrentSceneName;

        Debug.Log($"{dataName} save");


        if (!DataManager.instance.Data.DandelionsInGame.ContainsKey(dataName))
        {
            Debug.Log($"{dataName} no entry found, adding diction");


            DataManager.instance.Data.DandelionsInGame.Add(dataName,true);
        }

    }
}
