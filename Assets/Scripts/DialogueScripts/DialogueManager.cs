using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Component And Object Refs

    GameObject uiObj;
    Image topBar;
    TextMeshProUGUI nameText;
    TextMeshProUGUI currentDandelions;
    TextMeshProUGUI friendScore;

    GameObject dialogueObj;
    TextMeshProUGUI dialogueText;

    GameObject choicesObj;
    GameObject choice1Obj;
    GameObject choice2Obj;
    TextMeshProUGUI choiceOne;
    TextMeshProUGUI choiceTwo;

    RectTransform selectionArrow;

    PlayerController playerController;


    #endregion

    private DialogueEvent[] currentDialogue;
    private GameObject interactingObject;

    private int dialogueIndex = 0;

    int _currentChoice = 1;
    int currentChoiceSelection
    {
        get { return _currentChoice; }
        set
        {
            _currentChoice = value;

            if(_currentChoice == 1)
            {
                
                selectionArrow.localPosition = new Vector3(-35, selectionArrow.localPosition.y, 0);
            }
            else if (_currentChoice == 2)
            {
                selectionArrow.localPosition = new Vector3(35, selectionArrow.localPosition.y, 0);
            }
        }

    }
    int choiceOneGoTo;
    int choiceTwoGoTo;


    PlayerInput uiActions;
    InputAction action;
    InputAction navigation;

    private void Awake()
    {
        uiActions = new PlayerInput();

        dialogueObj = GameObject.Find("Dialogue");
        dialogueText = dialogueObj.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        choicesObj = GameObject.Find("Choices").gameObject;

        choice1Obj = GameObject.Find("ChoiceOneText");
        choice2Obj = GameObject.Find("ChoiceTwoText");

        choiceOne = choice1Obj.GetComponent<TextMeshProUGUI>();
        choiceTwo = choice2Obj.GetComponent<TextMeshProUGUI>();
        selectionArrow = choicesObj.transform.Find("SelectionArrow").gameObject.GetComponent<RectTransform>();

        uiObj = GameObject.Find("UI");
        
        topBar = uiObj.transform.Find("BG").GetComponent<Image>();
        nameText = uiObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        currentDandelions = uiObj.transform.Find("Score").GetComponent<TextMeshProUGUI>();
        friendScore = uiObj.transform.Find("ScoreMax").GetComponent<TextMeshProUGUI>();


    }
    private void OnEnable()
    {
        action = uiActions.UI.Action;
        navigation = uiActions.UI.Navigate;
        
    }

    private void OnDisable()
    {
        action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        dialogueObj.SetActive(false);
        choicesObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(DialogueEvent[] events, GameObject speakingWith, int startingIndex)
    {

        dialogueObj.SetActive(true);

        action.Enable();
        navigation.Enable();

        playerController = FindAnyObjectByType<PlayerController>();
        playerController.CurrentMoveRestrict = PlayerController.MovementRestrictions.InDialogue;

        currentDialogue = events;
        interactingObject = speakingWith;



        dialogueIndex = startingIndex;

        PlayDialogue();
    }

    private void CloseDialogue()
    {
        dialogueObj.SetActive(false);

        action.Disable();
        navigation.Enable();

        playerController = FindAnyObjectByType<PlayerController>();
        playerController.CurrentMoveRestrict = PlayerController.MovementRestrictions.NoRestriction;

        DataManager.instance.SaveGame();
    }

    public void PlayDialogue()
    {
        dialogueObj.SetActive(false);
        choicesObj.SetActive(false);


        if (dialogueIndex >= currentDialogue.Length)
        {
            CloseDialogue();

        }
        else
        {


            DialogueEvent nextEvent = currentDialogue[dialogueIndex];

            //Debug.Log($"Playing Event number {dialogueIndex}. Type: {currentDialogue[dialogueIndex].Type}");

            switch (nextEvent.Type)
            {
                case DialogueEvent.DialogueEventType.BasicDialogue:
                    dialogueObj.SetActive(true);
                    StartCoroutine(DisplayDialogue(nextEvent.TextSpeed, nextEvent.DialogueText));


                    break;


                case DialogueEvent.DialogueEventType.CollectDandelion:
                    dialogueObj.SetActive(true);
                    CollectDandelion();


                    break;

                case DialogueEvent.DialogueEventType.GiftDandelion:

                    GiftDandelion();

                    break;

                case DialogueEvent.DialogueEventType.Choice:

                    if (nextEvent.checkDandelion)
                    {
                        if(DataManager.instance.Data.CurrentDandelions < 1)
                        {
                            Debug.Log("Not enough Dandelions, ending dialogue");
                            CloseDialogue();
                            break;
                        }
                    }

                    choicesObj.SetActive(true);
                    
                    StartCoroutine(ChoiceStart(nextEvent));

                    break;

                case DialogueEvent.DialogueEventType.GoToIndex:

                    dialogueIndex = nextEvent.PageToChangeTo;
                    
                    PlayDialogue();

                    break;

                case DialogueEvent.DialogueEventType.SetStartIndex:
                    interactingObject.GetComponent<DialogueComponent>().StartingEventIndex = nextEvent.PageToChangeTo;

                    dialogueIndex++;

                    PlayDialogue();

                    break;

                case DialogueEvent.DialogueEventType.EndDialogue:

                    CloseDialogue();
                    break;

                case DialogueEvent.DialogueEventType.GlitchMap:

                    StartCoroutine(GlitchOverTime(nextEvent.TextSpeed));

                    break;

                case DialogueEvent.DialogueEventType.QuitGame:

                    Debug.Log("Quit Triggered");

                    Application.Quit();

                    break;
                case DialogueEvent.DialogueEventType.ChangeScene:

                    DataManager.instance.LoadScene(nextEvent.ChoiceOneText, nextEvent.ChoiceTwoText);

                    CloseDialogue();

                    break;

                case DialogueEvent.DialogueEventType.BurnFlower:

                    BurnFlower();

                    
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

        action.performed += ContinueDialogue;

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
            action.performed -= ContinueDialogue;

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
    }

    

    private void CollectDandelion()
    {
        string dataName = interactingObject.gameObject.name + DataManager.instance.Data.CurrentSceneName;

        if (DataManager.instance.Data.DandelionsInGame.ContainsKey(dataName))
        {
            Debug.Log($"Setting {dataName} to false (picked up)");

            DataManager.instance.Data.DandelionsInGame[dataName] = false;
        }

        DataManager.instance.Data.CurrentDandelions++;

        currentDandelions.text = DataManager.instance.Data.CurrentDandelions.ToString();

        Destroy(interactingObject, .1f);

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

    private IEnumerator ChoiceStart(DialogueEvent _event)
    {
        choiceOneGoTo = _event.ChoiceOneDestination;
        choiceTwoGoTo = _event.ChoiceTwoDestination;

        

        choiceOne.text = string.Empty;
        choiceTwo.text = string.Empty;
        char[] charArray = _event.ChoiceOneText.ToCharArray();

        writingDialogue = true;

        
        // write choice 1
        foreach (char c in charArray)
        {
            if (!writingDialogue)
            {
                break;
            }
            else
            {
                choiceOne.text += c;
                yield return new WaitForSeconds((float)_event.TextSpeed / 60);
            }

        }


        // write choice 2
        charArray = _event.ChoiceTwoText.ToCharArray();

        foreach (char c in charArray)
        {
            if (!writingDialogue)
            {
                break;
            }
            else
            {
                choiceTwo.text += c;
                yield return new WaitForSeconds((float)_event.TextSpeed / 60);
            }

        }

        selectionArrow.gameObject.SetActive(true);

        action.performed += ChoiceEnter;
        navigation.performed += ChoiceSwitch;

        writingDialogue = false;
    }

    private void ChoiceSwitch(InputAction.CallbackContext context)
    {
        if(currentChoiceSelection == 1)
        {
            currentChoiceSelection = 2;
        }
        else
        {
            currentChoiceSelection = 1;
        }
    }

    private void ChoiceEnter(InputAction.CallbackContext context)
    {
        selectionArrow.gameObject.SetActive(false);

        if(currentChoiceSelection == 1)
        {
            //Debug.Log($"Choice one hit, gping to {choiceOneGoTo}");

            dialogueIndex = choiceOneGoTo;
            
        }
        else if (currentChoiceSelection == 2)
        {
            //Debug.Log($"Choice two hit, gping to {choiceTwoGoTo}");
            dialogueIndex = choiceTwoGoTo;
        }

        action.performed -= ChoiceEnter;
        navigation.performed -= ChoiceSwitch;

        PlayDialogue();

    }

    public void ChangeUiColors(Color newColor)
    {
        dialogueText.color = newColor;
        choiceOne.color = newColor;
        choiceTwo.color = newColor;
        nameText.color = newColor;
        currentDandelions.color = newColor;
        friendScore.color = newColor;
        topBar.color = newColor;
        selectionArrow.GetComponent<Image>().color = newColor;
    }

    IEnumerator GlitchOverTime(float speed)
    {

        GlitchMap glitch = GameObject.FindAnyObjectByType<GlitchMap>();

        for (int i = 0; i < 5; i++)
        {
            glitch.RandomizeTiles();

            yield return new WaitForSeconds(speed);
        }

        dialogueIndex++;
        PlayDialogue();
    }

    void GiftDandelion()
    {
        DataManager.instance.Data.GiftedDandelions++;
        DataManager.instance.Data.CurrentDandelions--;

        currentDandelions.text = DataManager.instance.Data.CurrentDandelions.ToString();
        friendScore.text = DataManager.instance.Data.GiftedDandelions.ToString();

        dialogueIndex++;
        PlayDialogue();
    }

    void BurnFlower()
    {
        DataManager.instance.Data.GiftedDandelions--;
        DataManager.instance.Data.CurrentDandelions--;

        currentDandelions.text = DataManager.instance.Data.CurrentDandelions.ToString();
        friendScore.text = DataManager.instance.Data.GiftedDandelions.ToString();

        dialogueIndex++;
        PlayDialogue();
    }

}
