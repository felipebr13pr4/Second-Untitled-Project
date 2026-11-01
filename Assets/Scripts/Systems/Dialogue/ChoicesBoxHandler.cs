using UnityEngine;

public class ChoicesBoxHandler : MonoBehaviour
{
    [SerializeField] private ChoicesBox[] m_choicesBox;
    [SerializeField] private GameObject[] m_components;

    private void OnEnable()
    {
        ChoicesBox.OnChoicePicked += HideWrap;
    }

    private void OnDisable()
    {
        ChoicesBox.OnChoicePicked -= HideWrap;
    }

    public void Initialize(ChoicesNode node)
    {
        foreach (GameObject comp in m_components)
        {
            comp.SetActive(true);
        }
        for (int i = 0; i < node.Options.Length; i++)
        {
            bool shouldActivate = true;
            string name = node.Options[i].Name;
            m_choicesBox[i].Button.interactable = true;

            if (node.Options[i].FlagRequired != EventFlags.None)
            {
                bool hide = false;

                if (!node.Options[i].ReverseRequirement)
                {
                    hide = !EventFlagsHolder.Get(node.Options[i].FlagRequired);
                }
                else
                {
                    hide = EventFlagsHolder.Get(node.Options[i].FlagRequired);
                }

                if (hide)
                {
                    if (!node.Options[i].ShowOnUnmet)
                    {
                        shouldActivate = false;
                    }
                    else
                    {
                        name = "???";
                        m_choicesBox[i].Button.interactable = false;
                    }
                }
            }
            m_choicesBox[i].Flag = node.Options[i].Flag;
            m_choicesBox[i].gameObject.SetActive(shouldActivate);
            m_choicesBox[i].Node = node.Options[i].Node;
            m_choicesBox[i].Text = name;
            m_choicesBox[i].DisableFlag = node.Options[i].DisableFlag;
        }
    }

    private void HideWrap(Node node) => Hide();

    private void Hide()
    {
        foreach (ChoicesBox box in m_choicesBox)
        {
            box.gameObject.SetActive(false);
        }
        foreach (GameObject comp in m_components)
        {
            comp.SetActive(false);
        }
    }
}
