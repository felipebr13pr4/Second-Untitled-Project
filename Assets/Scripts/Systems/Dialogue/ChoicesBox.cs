using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesBox : MonoBehaviour
{
    [SerializeField] private Button m_button;
    public Button Button => m_button;
    [SerializeField] private TextMeshProUGUI m_text;
    public string Text { set { m_text.text = value; } }
    private Node m_node;
    public Node Node { set => m_node = value; }
    private EventFlags m_flag;
    public EventFlags Flag { set => m_flag = value; }
    private bool m_disableFlag = true;
    public bool DisableFlag { set => m_disableFlag = value; }

    public static event Action<Node> OnChoicePicked;

    private void OnEnable() => m_button.onClick.AddListener(ButtonClicked);

    private void OnDisable() => m_button.onClick.RemoveListener(ButtonClicked);

    private void ButtonClicked()
    {
        if (m_flag != EventFlags.None)
            EventFlagsHolder.Set(m_flag, !m_disableFlag);
        OnChoicePicked?.Invoke(m_node);
    }
}