using UnityEngine;
using System;

[Serializable]
public struct Option
{
    [SerializeField] private string m_name;
    public readonly string Name => m_name;
    [SerializeField] private Node m_node;
    public readonly Node Node => m_node;
    [SerializeField] private EventFlags m_flag;
    public readonly EventFlags Flag => m_flag;
    [SerializeField] private EventFlags m_flagRequired;
    public readonly EventFlags FlagRequired => m_flagRequired;
    [SerializeField] private bool m_disableFlag;
    public readonly bool DisableFlag => m_disableFlag;
    [SerializeField] private bool m_showOnUnmet;
    public readonly bool ShowOnUnmet => m_showOnUnmet;
    [SerializeField] private bool m_reverseRequirement;
    public readonly bool ReverseRequirement => m_reverseRequirement;

}