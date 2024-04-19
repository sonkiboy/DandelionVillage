using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GlitchMap : MonoBehaviour
{
    public int GlitchRadius = 1;


    [SerializeField] TileBase[] GlitchTiles;


    public Tilemap GroundMap;

    PlayerInput inputActions;
    InputAction action;

    private void OnEnable()
    {
        action.Enable();
    }

    private void Awake()
    {
        inputActions = new PlayerInput();
        action = inputActions.UI.Glitch;
        action.performed += RandomizeTiles;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RandomizeTiles(InputAction.CallbackContext context)
    {
        if(GlitchTiles.Length == 0)
        {
            Debug.LogError("Glitch tiles not assigned");
            return;
        }

        Vector3Int playerPos = Vector3Int.RoundToInt(GameObject.FindAnyObjectByType<PlayerController>().transform.position);

  
        for (int xPos = -(Mathf.RoundToInt(GlitchRadius/2)); xPos < (Mathf.RoundToInt(GlitchRadius / 2)); xPos++)
        {
            for (int yPos = -(Mathf.RoundToInt(GlitchRadius / 2)); yPos < (Mathf.RoundToInt(GlitchRadius / 2)); yPos++)
            {
                int randomNumber = Random.Range(0, GlitchTiles.Length);

                GroundMap.SetTile(new Vector3Int(xPos + playerPos.x, yPos + playerPos.y), GlitchTiles[randomNumber]);
            }

            

            

        }

    }

    
}
