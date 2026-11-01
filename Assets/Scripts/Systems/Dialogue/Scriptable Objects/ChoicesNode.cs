using UnityEngine;

[CreateAssetMenu(fileName = "New Choice Node", menuName = "Dialogue/Choice")]
public class ChoicesNode : Node
{
    [SerializeField] private Option[] m_options = new Option[1];
    public Option[] Options => m_options;

    private void OnValidate()
    {
        if (m_options.Length <= 0)
            m_options = new Option[1];
    }
}
