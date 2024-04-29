using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Diagnostics.Tracing;
using UnityEngine.InputSystem.Utilities;
using System;


// SAVE AND LOAD SYSTEM TAKEN FROM "SHAPED BY RAIN STUDIOS" ON YOUTUBE: https://youtu.be/aUi9aijvpgs?si=zG_XG2aithbQpy7E https://youtu.be/ijVA5Z-Mbh8?si=4_8k6C5QAXMqG7HU
public class DataManager : MonoBehaviour
{

    List<IDataPersistance> dataPersistances = new List<IDataPersistance>();

    public UITransision transition;
   
    public GameData Data;

    public static DataManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.Log("found existing Data Manager instance, deleting new one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        transition = GameObject.FindAnyObjectByType<UITransision>();

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistances = GetAllDataObjects();
        GetAllDandelions();
        
        LoadGame();
    }

    void OnSceneUnloaded(Scene scene)
    {
        
        SaveGame();
    }
    private void Start()
    {
        
    }

    public void NewGame()
    {
        Data = new GameData();
        LoadGame();
    }

    public void SaveGame()
    {
        

        foreach(IDataPersistance obj in dataPersistances)
        {
            obj.SaveData(ref instance.Data);
        }

        //Debug.Log($"Data saved | Scene: {Data.CurrentScene} Spawn: {Data.SpawnPointName}");
    }

    public void LoadGame()
    {


        foreach (IDataPersistance obj in dataPersistances)
        {
            obj.LoadData(instance.Data);
        }
        StartCoroutine(transition.FadeIn(1));


    }

    private List<IDataPersistance> GetAllDataObjects()
    {
        
        IEnumerable<IDataPersistance> objs = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        List<IDataPersistance> list = new List<IDataPersistance>(objs);

        //Debug.Log($"Found {list.Count}");

        return list;
    }

    // gets all pickups in the scene and stores them in the dictionary with their starting location
    private void GetAllDandelions()
    {
        

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Dandelions");

        foreach(GameObject obj in objs)
        {
            if (!instance.Data.DandelionsInGame.ContainsKey(obj))
            {
                instance.Data.DandelionsInGame.Add(obj, true);
            }
            
        }
    }

    public void LoadScene(string SpawnPointName, string GoToScene)
    {
        StartCoroutine(TransitionScene(SpawnPointName, GoToScene));


    }
    

    IEnumerator TransitionScene(string SpawnPointName, string GoToScene)
    {



        // put the spawn point name into the data manager script so the player controller can use it
        DataManager.instance.Data.SpawnPointName = SpawnPointName;

        UITransision transision = FindAnyObjectByType<UITransision>();

        StartCoroutine(transision.FadeOut(1));

        while (transision.isFading)
        {
            yield return null;
        }

        DataManager.instance.SaveGame();
        
        yield return null;
        
        // load the scene
        SceneManager.LoadScene(GoToScene);

        //Debug.Log($"Data: held obj {DataManager.heldObj.name}");

    }


}
