using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DialogueVarNames;

public class DialogueEditorWindow : EditorWindow
{
    private const string HolderStringKey = "ExpressionsHolder";
    private AudioClip m_defaultVoice;
    private ExpressionsHolder m_holder;
    private DialogueNode m_node = null;
    private Node m_next = null;
    private List<bool> m_showOptions;
    private List<bool> m_showExpression;
    private List<bool> m_showVoices;
    private List<bool> m_showSounds;
    private List<bool> m_showSkipping;
    private bool m_showAllExp;
    private bool m_showAllVoi;
    private bool m_showAllSou;
    private bool m_showAllSki;
    public enum ToolOperations
    {
        Add = 0,
        Remove = 1,
        Duplicate = 2,
    }
    private ToolOperations m_operation;
    private bool m_shouldDoOperation;
    private int m_lineIndex;
    private Vector2 m_scrollPos = Vector2.zero;
    private readonly GUILayoutOption[] m_defaultGui = new[]
                {
                GUILayout.MinWidth(20),
                GUILayout.MaxWidth(300),
                GUILayout.Height(175),
                };
    private readonly GUILayoutOption[] m_defaultButtonSize = new[]
    {
        GUILayout.Width(80),
        GUILayout.Height(30),
        GUILayout.MinWidth(20),
        GUILayout.MaxWidth(100),
        GUILayout.MinHeight(20),
        GUILayout.MaxHeight(40),
    };

    [MenuItem("Window/Dialogue Editor")]
    public static void Open()
    {
        GetWindow<DialogueEditorWindow>("Dialogue Editor");
    }

    private void OnEnable()
    {
        if (m_holder == null)
            m_holder = Resources.Load<ExpressionsHolder>("Main_Char_Expression_Holder");
       
        if (EditorPrefs.HasKey(HolderStringKey))
        {
            string json = EditorPrefs.GetString(HolderStringKey);
            m_holder = (ExpressionsHolder)CreateInstance("ExpressionsHolder");
            EditorJsonUtility.FromJsonOverwrite(json, m_holder);
        }
        
        m_defaultVoice = Resources.Load<AudioClip>("DefaultVoice");

        UpdateNext();
    }

    private void OnSelectionChange()
    {
        if (Selection.activeObject is DialogueNode node && node != m_node)
        {
            m_node = node;
            ClearBools();
            InitializeBools(m_node.Lines.Length);
            UpdateNext();
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (m_node != null)
        {
            SerializedObject serializedObject = new(m_node);

            serializedObject.Update();

            EditorUtility.SetDirty(m_node);

            m_holder = (ExpressionsHolder)EditorGUILayout.ObjectField(
    "Expression Holder", m_holder, typeof(ExpressionsHolder), false);
            
            if (m_holder == null)
            {
                return;
            }
            else
            {
                string holderJson = EditorJsonUtility.ToJson(m_holder);
                EditorPrefs.SetString(HolderStringKey, holderJson);
            }

            m_next = (Node)EditorGUILayout.ObjectField(
        "Next Node", m_next, typeof(Node), false);
            if (m_next != null)
                m_node.Next = m_next;

            while (m_showOptions.Count < m_node.Lines.Length) EditBools(true);
            while (m_showOptions.Count > m_node.Lines.Length) EditBools(false, m_showOptions.Count - 1);

            SerializedProperty lines = serializedObject.FindProperty("m_lines");

            List<DialogueLine> saveLines = new();
            foreach (DialogueLine line in m_node.Lines)
            {
                saveLines.Add(line);
            }

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            
            for (int i = 0; i < m_node.Lines.Length; i++)
            {
                EditorGUILayout.Space(5);

                SerializedProperty line = lines.GetArrayElementAtIndex(i);

                SerializedProperty text = line.FindPropertyRelative("m_text");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
                GUILayout.Label($"Line {i + 1}");
                EditorGUILayout.PropertyField(text, GUILayout.ExpandWidth(true), GUILayout.Width(300), GUILayout.MinWidth(300), GUILayout.MaxWidth(475));
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    m_shouldDoOperation = true;
                    m_operation = ToolOperations.Add;
                    m_lineIndex = i;
                }
                if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    m_shouldDoOperation = true;
                    m_operation = ToolOperations.Remove;
                    m_lineIndex = i;
                }
                if (GUILayout.Button("++", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    m_shouldDoOperation = true;
                    m_operation= ToolOperations.Duplicate;
                    m_lineIndex = i;
                }
                if (GUILayout.Button($"<{TagsConvention.Speed[..^1]}>", GUILayout.Width(55), GUILayout.Height(15)))
                {
                    m_node.Lines[i].Text += $"<{TagsConvention.Speed}{TextBox.DefaultTypingSpeed}>";
                }
                if (GUILayout.Button($"<{TagsConvention.Sound[..^1]}>", GUILayout.Width(55), GUILayout.Height(15)))
                {
                    m_node.Lines[i].Text += $"<{TagsConvention.Sound}>";
                    if (m_node.Lines[i].Sounds == null || m_node.Lines[i].Sounds.Length == 0)
                        m_node.Lines[i].Sounds = new AudioData[1];
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(140), GUILayout.Height(140));
                if (m_node.Lines[i].Expression.AssignedExpression != Expressions.None)
                {
                    Texture2D portraitTexture = m_holder.GetExpressionSprite(m_node.Lines[i].Expression.AssignedExpression).Value.Portrait.texture;
                    GUILayout.Label(portraitTexture, GUILayout.MaxWidth(m_node.Lines[i].Expression.HasFullBody ? 100 : 140), GUILayout.MaxHeight(140));
                    if (m_node.Lines[i].Expression.HasFullBody)
                    {
                        ExpressionSprites? sprites = m_holder.GetExpressionSprite(m_node.Lines[i].Expression.AssignedExpression);
                        if (sprites != null)
                        {
                            Texture2D fullbodyTexture = sprites.Value.FullBody.texture;
                            GUILayout.Label(fullbodyTexture, GUILayout.MaxWidth(40), GUILayout.MaxHeight(140));
                        }
                    }
                }
                else
                {
                    GUILayout.Space(140);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("box", GUILayout.MaxHeight(140));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Options", GUILayout.Width(80), GUILayout.Height(90)))
                {
                    m_showOptions[i] = !m_showOptions[i];
                    m_showExpression[i] = false;
                    m_showVoices[i] = false;
                    m_showSounds[i] = false;
                    m_showSkipping[i] = false;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.MaxHeight(140));
                GUILayout.FlexibleSpace();

                if (m_showOptions[i])
                {
                    EditorGUILayout.BeginVertical("box");
                    if (GUILayout.Button("Expressions", m_defaultButtonSize))
                        m_showExpression[i] = !m_showExpression[i];
                    if (GUILayout.Button("Voices", m_defaultButtonSize))
                        m_showVoices[i] = !m_showVoices[i];
                    if (GUILayout.Button("Sounds", m_defaultButtonSize))
                        m_showSounds[i] = !m_showSounds[i];
                    if (GUILayout.Button("Skipping", m_defaultButtonSize))
                        m_showSkipping[i] = !m_showSkipping[i];
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginHorizontal();

                if (m_showExpression[i])
                {
                    SerializedProperty expression = line.FindPropertyRelative(C_Expression);
                    EditorGUILayout.BeginVertical("box");
                    GUILayout.Label($"Expression");
                    EditorGUILayout.PropertyField(expression, m_defaultGui);
                    EditorGUILayout.EndVertical();
                }

                if (m_showVoices[i])
                {
                    SerializedProperty voices = line.FindPropertyRelative(C_Voice);
                    EditorGUILayout.BeginVertical("box");
                    GUILayout.Label($"Voice");
                    EditorGUILayout.PropertyField(voices, m_defaultGui);
                    EditorGUILayout.EndVertical();
                }

                if (m_showSounds[i])
                {
                    SerializedProperty sounds = line.FindPropertyRelative(C_Sounds);
                    EditorGUILayout.BeginVertical("box");
                    GUILayout.Label($"Sounds");
                    EditorGUILayout.PropertyField(sounds, m_defaultGui);
                    EditorGUILayout.EndVertical();
                }

                if (m_showSkipping[i])
                {
                    SerializedProperty skips = line.FindPropertyRelative(C_AutoSkip);
                    EditorGUILayout.BeginVertical("box", m_defaultGui);
                    GUILayout.Label($"Skips");
                    EditorGUILayout.PropertyField(skips);
                    if (skips.boolValue)
                    {
                        SerializedProperty delay = line.FindPropertyRelative(C_Delay);
                        EditorGUILayout.PropertyField(delay);
                    }
                    SerializedProperty unskippable = line.FindPropertyRelative(C_Unskippable);
                    EditorGUILayout.PropertyField(unskippable);
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();
            }

            if (m_shouldDoOperation)
            {
                switch (m_operation)
                {
                    case ToolOperations.Add:
                        saveLines.Insert(m_lineIndex + 1, new());
                        m_node.Lines = saveLines.ToArray();
                        m_shouldDoOperation = false;
                        m_lineIndex = -1;
                        break;

                    case ToolOperations.Remove:
                        saveLines.RemoveAt(m_lineIndex);
                        m_node.Lines = saveLines.ToArray();
                        m_shouldDoOperation = false;
                        EditBools(false, m_lineIndex);
                        m_lineIndex = -1;
                        break;

                    case ToolOperations.Duplicate:
                        saveLines.Insert(m_lineIndex + 1, m_node.Lines[m_lineIndex]);
                        m_node.Lines = saveLines.ToArray();
                        m_shouldDoOperation = false;
                        DuplicateAll(m_lineIndex);
                        m_lineIndex = -1;
                        break;

                    default:
                        ErrorLogger.LogError("Something went wrong, operation type received but not detected by any of the existent ones. Make sure that the switch is up to date");
                        break;
                }
            }
            
            while (m_showOptions.Count < m_node.Lines.Length) EditBools(true);
            while (m_showOptions.Count > m_node.Lines.Length) EditBools(false, m_showOptions.Count - 1);
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            GUILayout.Label("Extras");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Last", m_defaultButtonSize))
            {
                saveLines.Add(new());
                m_node.Lines = saveLines.ToArray();
            }
            if (GUILayout.Button("Remove Last", m_defaultButtonSize))
            {
                saveLines.RemoveAt(saveLines.Count - 1);
                m_node.Lines = saveLines.ToArray();
            }
            if (GUILayout.Button("Clone Last", m_defaultButtonSize))
            {
                saveLines.Add(m_node.Lines[^1]);
                m_node.Lines = saveLines.ToArray();
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Show Alls");
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Show Exp.", m_defaultButtonSize))
            {
                m_showAllExp = !m_showAllExp;
                ExpandElement(C_Expression, m_showAllExp, m_showExpression.Count, ref lines);
                SetBoolList(ref m_showOptions, true);
                SetBoolList(ref m_showExpression, m_showAllExp);
            }

            if (GUILayout.Button("Show Voices", m_defaultButtonSize))
            {
                m_showAllVoi = !m_showAllVoi;
                ExpandElement(C_Voice, m_showAllVoi, m_showVoices.Count, ref lines);
                SetBoolList(ref m_showOptions, true);
                SetBoolList(ref m_showVoices, m_showAllVoi);
            }

            if (GUILayout.Button("Show Sounds", m_defaultButtonSize))
            {
                m_showAllSou = !m_showAllSou;
                ExpandElement(C_Sounds, m_showAllSou, m_showSounds.Count, ref lines);
                SetBoolList(ref m_showOptions, true);
                SetBoolList(ref m_showSounds, m_showAllSou);
            }

            if (GUILayout.Button("Show Skips", m_defaultButtonSize))
            {
                m_showAllSki = !m_showAllSki;
                ExpandElement(C_AutoSkip, m_showAllSki, m_showSkipping.Count, ref lines);
                SetBoolList(ref m_showOptions, true);
                SetBoolList(ref m_showSkipping, m_showAllSki);
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Change Alls");
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("None Portraits", m_defaultButtonSize))
            {
                SetAllPortraits(Expressions.None);
            }
            if (GUILayout.Button("Neutral Portraits", m_defaultButtonSize))
            {
                SetAllPortraits(Expressions.Neutral);
            }
            if (GUILayout.Button("False Bodies", m_defaultButtonSize))
            {
                SetAllBodies(false);
            }
            if (GUILayout.Button("True Bodies", m_defaultButtonSize))
            {
                SetAllBodies(true);
            }
            if (GUILayout.Button("None Voices", m_defaultButtonSize))
            {
                for (int i = 0; i < m_node.Lines.Length; i++)
                    m_node.Lines[i].Voice = new();
            }
            if (GUILayout.Button("Default Voices", m_defaultButtonSize))
            {
                for (int i = 0; i < m_node.Lines.Length; i++)
                    m_node.Lines[i].Voice = new(m_defaultVoice, 1, true, 1f, 1.5f);
            }
            if (GUILayout.Button("No Sounds", m_defaultButtonSize))
            {
                for (int i = 0; i < m_node.Lines.Length; i++)
                    m_node.Lines[i].Sounds = new AudioData[0];
            }
            if (GUILayout.Button("4 Sounds", m_defaultButtonSize))
            {
                for (int i = 0; i < m_node.Lines.Length; i++)
                    m_node.Lines[i].Sounds = new AudioData[4];
            }
            if (GUILayout.Button("Auto Skip", m_defaultButtonSize))
            {
                for (int i = 0; i < m_node.Lines.Length; i++)
                    m_node.Lines[i].AutoSkip = !m_node.Lines[i].AutoSkip;
            }
            if (GUILayout.Button("Unskippable", m_defaultButtonSize))
            {
                for (int i = 0; i < m_node.Lines.Length; i++)
                    m_node.Lines[i].Unskippable = !m_node.Lines[i].Unskippable;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            GUILayout.Label("Please select a dialogue node.");
        }

    }

    private void EditBools(bool add, int i = 0)
    {
        if (add)
        {
            m_showOptions.Add(false);
            m_showExpression.Add(false);
            m_showVoices.Add(false);
            m_showSounds.Add(false);
            m_showSkipping.Add(false);
        }
        else
        {
            m_showOptions.RemoveAt(i);
            m_showExpression.RemoveAt(i);
            m_showVoices.RemoveAt(i);
            m_showSounds.RemoveAt(i);
            m_showSkipping.RemoveAt(i);
        }
    }


    private void DuplicateAll(int i = 0)
    {
        Duplicate(ref m_showOptions, i);
        Duplicate(ref m_showExpression, i);
        Duplicate(ref m_showVoices, i);
        Duplicate(ref m_showSounds, i);
        Duplicate(ref m_showSkipping, i);
    }

    private void Duplicate(ref List<bool> list, int i = 0) => list.Insert(i, list[i]);


    private void ClearBools()
    {
        m_showOptions?.Clear();
        m_showExpression?.Clear();
        m_showVoices?.Clear();
        m_showSounds?.Clear();
        m_showSkipping?.Clear();
    }

    private void InitializeBools(int value)
    {
        m_showOptions = new(value);
        m_showExpression = new(value);
        m_showVoices = new(value);
        m_showSounds = new(value);
        m_showSkipping = new(value);
    }

    private void SetBoolList(ref List<bool> list, bool state)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = state;
        }
    }

    private void ExpandElement(string name, bool state, int lenght, ref SerializedProperty lines)
    {
        for (int i = 0; i < lenght; i++)
        {
            SerializedProperty line = lines.GetArrayElementAtIndex(i);
            SerializedProperty target = line.FindPropertyRelative(name);
            target.isExpanded = state;
        }
    }

    private void SetAllPortraits(Expressions expression)
    {
        for (int i = 0; i < m_node.Lines.Length; i++)
            m_node.Lines[i].Expression = new(expression, m_node.Lines[i].Expression.HasFullBody);
    }

    private void SetAllBodies(bool hasFullBody)
    {
        for (int i = 0; i < m_node.Lines.Length; i++)
            m_node.Lines[i].Expression = new(m_node.Lines[i].Expression.AssignedExpression, hasFullBody);
    }

    private void UpdateNext()
    {
        m_next = m_node != null ? m_node.Next : null;
    }
}