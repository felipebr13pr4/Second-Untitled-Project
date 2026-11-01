#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation Node", menuName = "Dialogue/Animation")]
public class AnimationNode : NodeWithNext
{
    [SerializeField] private string m_state;
    public string State => m_state;
    [SerializeField] private bool m_keep;
    public bool Keep => m_keep;
    [SerializeField] private bool m_disable;
    public bool Disable => m_disable;
    [SerializeField] private float m_time;
    public float Time => m_time;
#if UNITY_EDITOR
    [SerializeField] private AnimatorController m_animator;
    private int m_warningIndex;

    private void OnEnable()
    {
        FillAnimator();
    }

    private void OnValidate()
    {
        FillAnimator();

        if (m_animator == null)
        {
            ErrorLogger.LogError($"No animator detected inside a Animator Node check. Make sure that the resources load is correctly typed and if you updated the animator controller (Special Player Anim) name to update it here. I am: {name}");
        }

        m_warningIndex++;
        if (m_warningIndex > 2)
        {
            if (!StateExists(m_animator, m_state))
            {
                ErrorLogger.LogWarning($"State detected inside a Animator Node that does not exist. Make sure that the string is correctly typed to a correspondent state in the animator controller. I am: {name}");
            }
        }
    }

    private void FillAnimator()
    {
        if (m_animator == null)
            m_animator = Resources.Load<AnimatorController>("Special_Player");
    }

    // Made by Claude. (I understand it, it is nice)
    private bool StateExists(AnimatorController controller, string stateName)
    {
        foreach (var layer in controller.layers)
        {
            foreach (var childState in layer.stateMachine.states)
            {
                if (childState.state.name == stateName)
                    return true;
            }
        }
        return false;
    }
#endif
}
