using System;
using UnityEngine;

[Serializable]
public struct FlagPersistence
{
    [SerializeField] private EventFlags m_flag;
    public readonly EventFlags Flag => m_flag;
    [SerializeField] private bool m_isPermanent;
    public readonly bool IsPermanent => m_isPermanent;

    public FlagPersistence(EventFlags flag, bool isPermanent)
    {
        m_flag = flag;
        m_isPermanent = isPermanent;
    }
}