using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueEvent 
{
    public enum DialogueEventType
    {
        BasicDialogue,
        CharacterDialogue,
        CollectDandelion,
        Choice,
        ChangePage,
        EndDialogue

    }

    public DialogueEventType Type;

    public int TextSpeed = 1;

    public string DialogueText;

    public string ChoiceOneText;
    public int ChoiceOneDestination;

    public string ChoiceTwoText;
    public int ChoiceTwoDestination;

    public int PageToChangeTo;
}
