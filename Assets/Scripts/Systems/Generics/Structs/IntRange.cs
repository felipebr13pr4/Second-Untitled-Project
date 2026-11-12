using System;
using UnityEngine;

[Serializable]
public struct IntRange
{
    [SerializeField] private int m_min;
    public readonly int Min => m_min;
    [SerializeField] private int m_max;
    public readonly int Max => m_max;

    public IntRange(int min, int max)
    {
        m_min = min;
        m_max = max;
    }
}