using UnityEngine;

public class NodeWithNext : Node
{
    [SerializeField] private Node m_next;
    public Node Next { get => m_next; set => m_next = value; }
}