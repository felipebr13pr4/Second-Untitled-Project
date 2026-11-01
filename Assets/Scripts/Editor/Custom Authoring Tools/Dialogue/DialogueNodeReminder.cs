using UnityEditor;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeReminder : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        GUILayout.Label("If you wish to edit this file, please open the dialogue editor window.\nLocated at the top unity bars.\nIn Window choose 'dialogue editor window' and insert this dialogue node in it.");

        serializedObject.ApplyModifiedProperties();
    }
}
