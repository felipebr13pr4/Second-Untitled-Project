using System;
using UnityEngine;

[Serializable]
public struct ExpressionSprites
{
    [SerializeField] private string m_name;
    public string Name { set => m_name = value; }
    [SerializeField] private Sprite m_portrait;
    public readonly Sprite Portrait => m_portrait;
    [SerializeField] private Sprite m_fullBody;
    public readonly Sprite FullBody => m_fullBody;
    [SerializeField] private Expressions m_expression;
    public readonly Expressions Expression => m_expression;
}