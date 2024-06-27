using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueEvent 
{
    public enum DialogueEventType
    {
        BasicDialogue,
        CharacterDialogue,
        CollectDandelion,
        GiftDandelion,
        Choice,
        GoToIndex,
        SetStartIndex,
        EndDialogue,
        GlitchMap,
        QuitGame,
        ChangeScene,
        BurnFlower

    }

    public DialogueEventType Type;

    public bool checkDandelion;

    public int TextSpeed;

    public string DialogueText;

    public string ChoiceOneText;
    public int ChoiceOneDestination;

    public string ChoiceTwoText;
    public int ChoiceTwoDestination;

    public int PageToChangeTo;
}
