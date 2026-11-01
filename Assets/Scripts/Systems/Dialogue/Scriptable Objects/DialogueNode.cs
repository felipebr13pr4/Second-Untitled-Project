using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue")]
public class DialogueNode : NodeWithNext
{
    [SerializeField] private DialogueLine[] m_lines = new DialogueLine[1];
    public DialogueLine[] Lines { get => m_lines; set => m_lines = value; }
    [SerializeField] private bool m_defaultVoice = true;
    private int m_warningIndex = 0;


    private void OnValidate()
    {
        m_warningIndex++;
        if (m_warningIndex > 2)
        {
            foreach (DialogueLine line in m_lines)
            {
                TagFunctions.Scan(line.Text);
            }
            m_warningIndex = 0;
        }

        if (m_lines.Length != 0)
        for (int i = 0; i < m_lines.Length; i++)
        {
            if (m_lines[i].Sounds != null)
                    for (int j = 0; j < m_lines[i].Sounds.Length; j++)
                    {
                        if (m_lines[i].Sounds[j].Clip != null)
                            m_lines[i].Sounds[j].Name = m_lines[i].Sounds[j].Clip.name;
                        else
                            m_lines[i].Sounds[j].Name = "None.";
                    }
            }

        if (m_defaultVoice)
        {
            for (int i = 0; i < m_lines.Length; i++)
            {
                m_lines[i].Voice = new(Resources.Load<AudioClip>("DefaultVoice"),
                    1, true, 1f, 1.5f);
            }
            m_defaultVoice = false;
        }
    }
}
