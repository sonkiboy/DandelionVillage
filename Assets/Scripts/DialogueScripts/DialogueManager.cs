using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    GameObject dialogueObj;
    TextMeshProUGUI dialogueText;
    PlayerController playerController;

    private DialogueEvent[] currentDialogue;
    private GameObject interactingObject;
    private int dialogueIndex;

    PlayerInput uiActions;
    InputAction action;

    private void Awake()
    {
        uiActions = new PlayerInput();

        dialogueObj = GameObject.Find("Dialogue");
        dialogueText = dialogueObj.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }
    private void OnEnable()
    {
        action = uiActions.UI.Action;
        
        action.performed += ContinueDialogue;
    }

    private void OnDisable()
    {
        action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        dialogueObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(DialogueEvent[] events, GameObject speakingWith, int startingIndex)
    {

        dialogueObj.SetActive(true);

        action.Enable();
        playerController.CurrentMoveRestrict = PlayerController.MovementRestrictions.InDialogue;

        currentDialogue = events;
        interactingObject = speakingWith;



        dialogueIndex = startingIndex;

        PlayDialogue();
    }

    public void PlayDialogue()
    {

        if (dialogueIndex >= currentDialogue.Length)
        {
            CloseDialogue();

        }
        else
        {


            DialogueEvent nextEvent = currentDialogue[dialogueIndex];

            switch (nextEvent.Type)
            {
                case DialogueEvent.DialogueEventType.BasicDialogue:

                    StartCoroutine(DisplayDialogue(nextEvent.TextSpeed, nextEvent.DialogueText));


                    break;


                case DialogueEvent.DialogueEventType.CollectDandelion:

                    CollectDandelion();


                    break;

                case DialogueEvent.DialogueEventType.Choice:

                    Choice();

                    break;

                case DialogueEvent.DialogueEventType.ChangeIndex:

                    dialogueIndex = nextEvent.IndexToChangeTo;
                    PlayDialogue();

                    break;
            }
        }

    }

    bool writingDialogue = false;
    private IEnumerator DisplayDialogue(int textSpeed, string dialogue)
    {
        dialogueText.text = string.Empty;
        char[] charArray = dialogue.ToCharArray();

        writingDialogue = true;
        
        foreach (char c in charArray)
        {
            if (!writingDialogue)
            {
                break;
            }
            else
            {
                dialogueText.text += c;
                yield return new WaitForSeconds((float)textSpeed / 60);
            }
            
        }

        writingDialogue = false;

    }

    private void ContinueDialogue(InputAction.CallbackContext context)
    {
        if (writingDialogue)
        {
            writingDialogue = false;

            dialogueText.text = currentDialogue[dialogueIndex].DialogueText;
        }
        else
        {
            

            if(dialogueIndex >= currentDialogue.Length)
            {
                CloseDialogue();
            }
            else
            {
                dialogueIndex++;

                PlayDialogue();
            }
            
        }
    }

    private void CloseDialogue()
    {
        dialogueObj.SetActive(false);

        action.Disable();
        playerController.CurrentMoveRestrict = PlayerController.MovementRestrictions.NoRestriction;
    }

    private void CollectDandelion()
    {
        if (DataManager.instance.Data.DandelionsInGame.ContainsKey(interactingObject))
        {
            DataManager.instance.Data.DandelionsInGame[interactingObject] = false;
        }

        DataManager.instance.Data.CurrentDandelions++;

        Destroy(interactingObject);

        if (dialogueIndex >= currentDialogue.Length)
        {
            CloseDialogue();
        }
        else
        {
            dialogueIndex++;



            PlayDialogue();
        }

    }

    private void Choice()
    {

    }


}
