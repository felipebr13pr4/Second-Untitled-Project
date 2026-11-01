using System;
using UnityEngine;

[Serializable]
public struct TextData
{
    [SerializeField] private string m_text;
    public readonly string Text => m_text;
    [SerializeField] private TagCue[] m_tags;
    public readonly TagCue[] Tags => m_tags;

    public TextData(string text, TagCue[] cues)
    {
        m_text = text;
        m_tags = cues;
    }
}