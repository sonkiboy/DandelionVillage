using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.SceneManagement;

public class ChangeSceneBehavior : MonoBehaviour
{
    [SerializeField] string GoToScene;
    [SerializeField] string SpawnPointName;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Scene trigger hit, found: {collision.gameObject.name}");

        if (collision.gameObject.tag == "Player")
        {
            if (GoToScene != null && SpawnPointName != null)
            {
                TransitionScene();
            }

            else
            {
                Debug.Log("Please input Scene name and Spawn name");
            }
        }
    }
   

    void TransitionScene() 
    {

        

        // put the spawn point name into the data manager script so the player controller can use it
        DataManager.instance.Data.SpawnPointName = SpawnPointName;



        DataManager.instance.SaveGame();

        // load the scene
        SceneManager.LoadScene(GoToScene);

        //Debug.Log($"Data: held obj {DataManager.heldObj.name}");

    }
}
