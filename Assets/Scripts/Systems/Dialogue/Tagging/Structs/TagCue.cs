using System;
using UnityEngine;

[Serializable]
public struct TagCue
{
    [SerializeField] private TagType m_tagType;
    public readonly TagType TagType => m_tagType;
    [SerializeField] private float m_value;
    public readonly float Value => m_value;
    public readonly int ValueInt => (int)m_value;
    [SerializeField] private int m_pos;
    public readonly int Pos => m_pos;

    public TagCue(TagType type, float value, int pos)
    {
        m_tagType = type;
        m_value = value;
        m_pos = pos;
    }
}