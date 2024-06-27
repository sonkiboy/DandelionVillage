using NUnit.Framework;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(DialogueComponent))]
public class DialogueComponentEditor : Editor
{
    DialogueComponent component;

    SerializedProperty dialogueList;

    SerializedProperty startIndex;

    ReorderableList displayedList;

    private void OnEnable()
    {
        component = target as DialogueComponent;

        dialogueList = serializedObject.FindProperty("ListOfEvents");

        startIndex = serializedObject.FindProperty("StartingEventIndex");

        displayedList = new ReorderableList(serializedObject, dialogueList, true, true, true, true);

        displayedList.drawElementBackgroundCallback = DrawListIndex;
    }

    void DrawListIndex(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty dialogueProperty = displayedList.serializedProperty.GetArrayElementAtIndex(index);


        DialogueEvent dialogue = component.ListOfEvents[index];

        
        if (isFocused)
        {
            EditorGUILayout.LabelField($"Element: {index}"); 

            //EditorGUI.PropertyField(new Rect(rect.x, rect.y - 10, 100, EditorGUIUtility.singleLineHeight), dialogueProperty.FindPropertyRelative("Type"));
            EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("Type"));


            DialogueEvent.DialogueEventType type = dialogue.Type;



            switch (type)
            {
                case DialogueEvent.DialogueEventType.BasicDialogue:
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("TextSpeed"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("DialogueText"));
                    
                    break;

                case DialogueEvent.DialogueEventType.Choice:
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("checkDandelion"));

                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("TextSpeed"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("DialogueText"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("ChoiceOneText"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("ChoiceOneDestination"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("ChoiceTwoText"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("ChoiceTwoDestination"));
                    break;
                case DialogueEvent.DialogueEventType.CollectDandelion:
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("TextSpeed"));
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("DialogueText"));
                    break;

                case DialogueEvent.DialogueEventType.SetStartIndex:
                    EditorGUILayout.PropertyField(dialogueProperty.FindPropertyRelative("PageToChangeTo"));

                    break;

            }
        }
    }


    public override void OnInspectorGUI()
    {

        
        //base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(startIndex);

        displayedList.DoLayoutList();


        serializedObject.ApplyModifiedProperties();
    }
}
