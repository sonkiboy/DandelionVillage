using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    public GameObject debugObj;

    PlayerInput inputActions;
    InputAction input;

    private void Awake()
    {
        inputActions = new PlayerInput();
        input = inputActions.UI.Debug;
        input.performed += ToggleDebugMenu;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToScene(string name)
    {
        Debug.Log("Button Hit");

        SceneManager.LoadScene(name);
    }

    public void Reset()
    {
        DataManager.instance.NewGame();
        SceneManager.LoadScene("VillageAndForest");
    }

    void ToggleDebugMenu(InputAction.CallbackContext context)
    {
        

        if (debugObj.activeSelf)
        {
            debugObj.SetActive(false);
        }
        else
        {
            debugObj.SetActive(true);
        }
    }
}
