using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDataPersistance
{
    #region Object and Componenet

    Animator animator;
    Rigidbody2D rb;

    DialogueManager dialogueManager;

    #endregion

    public enum MovementRestrictions
    {
        NoRestriction,
        NoMovement,
        InDialogue,

    }

    private MovementRestrictions _currentRestrict = MovementRestrictions.NoRestriction;
    public MovementRestrictions CurrentMoveRestrict
    {
        get { return _currentRestrict; }
        set 
        { 
            _currentRestrict = value;

            if (animator != null)
            {
                animator.SetTrigger("TrIdle");
            }

            
            
            if (value == MovementRestrictions.InDialogue)
            {
                action.Disable();
            }
            else
            {
                action.Enable();
            }
        }
    }

    public enum Direction
    {
        Up,Down,Left,Right
    }

    private Direction _currentDirection = Direction.Down;
    public Direction CurrentDirection
    {
        get { return _currentDirection; }
        set { _currentDirection = value; }
    }

    public float Speed = 1;

    


    PlayerInput playerControls;
    InputAction move;
    InputAction action;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dialogueManager = GameObject.Find("Canvas").GetComponent<DialogueManager>();

        playerControls = new PlayerInput();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        action = playerControls.Player.Action;
        action.Enable();
        action.performed += Action;

        
    }

    private void OnDisable()
    {
        move.Disable();
    }


    public void LoadData(GameData data)
    {
        if (DataManager.instance.Data.SpawnPointName != string.Empty)
        {
            Transform spawnTransform = GameObject.Find(DataManager.instance.Data.SpawnPointName).transform.Find("SpawnPoint");

            this.transform.position = spawnTransform.position;
            this.transform.rotation = spawnTransform.rotation;
        }
        
    }

    public void SaveData(ref GameData data)
    {

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        

        Vector2 movePos = new Vector2(Mathf.RoundToInt(move.ReadValue<Vector2>().x), Mathf.RoundToInt(move.ReadValue<Vector2>().y));

        if(CurrentMoveRestrict != MovementRestrictions.NoRestriction)
        {
            movePos = Vector2.zero;
        }

        // set the animator to the right state
        if(movePos == Vector2.zero)
        {
            animator.SetTrigger("TrIdle");
        }
        else
        {
            if(movePos.y == 1)
            {
                CurrentDirection = Direction.Up;
                animator.SetTrigger("TrUp");
                movePos = Vector2.up;
            }
            else if(movePos.y == -1 )
            {
                CurrentDirection = Direction.Down;

                animator.SetTrigger("TrDown");
                movePos = Vector2.down;
            }
            else if(movePos.x == 1 )
            {
                CurrentDirection = Direction.Right;

                animator.SetTrigger("TrRight");
                movePos = Vector2.right;
            }
            else if(movePos.x == -1)
            {
                CurrentDirection = Direction.Left;

                animator.SetTrigger("TrLeft");
                movePos = Vector2.left;
            }
        }


        movePos = rb.position + (movePos * Speed * Time.deltaTime);
        rb.MovePosition(movePos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

    }

    public void MoveToRoom()
    {
        Vector2 offset = rb.position;
        switch (CurrentDirection)
        {
            case Direction.Up:
                offset += Vector2.up ;
                break;

            case Direction.Down:
                offset += Vector2.down ;

                break;

            case Direction.Left:
                offset += Vector2.left ;

                break;

            case Direction.Right:
                offset += Vector2.right ;

                break;
        }

        //Debug.Log($"Entering new room, moving player from {rb.position} to {offset}");
        transform.position = (offset);
    }

    private void Action(InputAction.CallbackContext context)
    {
        

        Vector2 checkPosition = (Vector2)this.transform.position + GetDirectionVector(CurrentDirection);

        Collider2D foundCollider = Physics2D.OverlapBox(checkPosition, Vector2.one, 0, LayerMask.GetMask("Interactable"));

        //Debug.Log($"Action input hit| Player Pos: {this.transform.position}| Checked Pos: {checkPosition}");

        if (foundCollider != null)
        {
            //Debug.Log("Player interact hit");

            DialogueEvent[] events = foundCollider.gameObject.GetComponent<DialogueComponent>().ListOfEvents;
            int index = foundCollider.gameObject.GetComponent<DialogueComponent>().StartingEventIndex;

            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(events,foundCollider.gameObject,index);
                
            }
        }
    }

    private Vector2 GetDirectionVector(Direction dir)
    {
        Vector2 vector = Vector2.zero;

        switch(dir)
        {
            case Direction.Up:
                vector = Vector2.up;
            break;

            case Direction.Down:
                vector = Vector2.down;
                break;

            case Direction.Left:
                vector = Vector2.left;

                break;

            case Direction.Right:
                vector = Vector2.right;

                break;
        }

        return vector;
    }
}
