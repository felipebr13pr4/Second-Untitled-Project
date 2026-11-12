using System;
using UnityEngine;

[Serializable]
public struct DieData
{
    [Range(-80, 80)]
    [SerializeField] private int m_baseValue;
    public readonly int BaseValue => m_baseValue;
    [SerializeField] private IntRange m_rangeValue;
    public readonly IntRange RangeValue => m_rangeValue;

    public DieData(int baseValue, IntRange rangeValue)
    {
        m_baseValue = baseValue;
        m_rangeValue = rangeValue;
    }
}