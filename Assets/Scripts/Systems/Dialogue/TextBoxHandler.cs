using UnityEngine;

public class TextBoxHandler : MonoBehaviour
{
    [SerializeField] private DialogueRouter m_router;

    private void OnEnable()
    {
        // Put here events that will cause dialogue to happen.
    }

    private void OnDisable()
    {
        // Put here events that will cause dialogue to happen.
    }

    private void Execute(Node node)
    {
        m_router.Initialize(node);
    }
}