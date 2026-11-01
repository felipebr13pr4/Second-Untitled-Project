using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct DialogueLine
{
    [TextArea(5, 1000)]
    [SerializeField] private string m_text;
    public string Text { readonly get => m_text; set => m_text = value; }
    [SerializeField] private LineExpression m_expression;
    public LineExpression Expression { readonly get => m_expression; set => m_expression = value; }
    [SerializeField] private AudioData m_voice;
    public AudioData Voice { readonly get => m_voice; set => m_voice = value; }
    [SerializeField] private AudioData[] m_sounds;
    public AudioData[] Sounds { readonly get => m_sounds; set => m_sounds = value; }
    [SerializeField] private bool m_autoSkip;
    public bool AutoSkip { readonly get => m_autoSkip; set => m_autoSkip = value; }
    [SerializeField] private float m_delay;
    public readonly float Delay => m_delay;
    [SerializeField] private bool m_unskippable;
    public bool Unskippable { readonly get => m_unskippable; set => m_unskippable = value; }

    public DialogueLine(string text, LineExpression expression, AudioData voice, AudioData[] sounds, bool autoSkip, float delay, bool unskippable)
    {
        m_text = text;
        m_expression = expression;
        m_voice = voice;
        m_sounds = sounds;
        m_autoSkip = autoSkip;
        m_delay = delay;
        m_unskippable = unskippable;
    }

    public readonly DialogueLine ChangeExpression(LineExpression newExpression)
    {
        DialogueLine copy = this;
        copy.m_expression = newExpression;
        return copy;
    }

    public readonly DialogueLine ChangeSkip(bool autoSkip, bool unskippable)
    {
        DialogueLine copy = this;
        copy.m_autoSkip = autoSkip;
        copy.m_unskippable = unskippable;
        return copy;
    }
}
