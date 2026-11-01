using UnityEngine;

public class TagHandler : MonoBehaviour
{
    [SerializeField] private DialogueAudioHandler m_audioHandler;
    [SerializeField] private TextBox m_textBox;
    [SerializeField] private AudioData[] m_audios;
    [SerializeField] private TextData m_textData;
    public TextData TextData
    { 
        get
        {
            return m_textData;
        }

        set
        {
            m_textData = value;
        }
    }
    [SerializeField] private int m_tagIndex;

    public void Initialize(string text, AudioData[] sounds)
    {
        m_textData = TagFunctions.Scan(text);
        m_tagIndex = 0;
        m_audios = sounds;
    }

    public void Check(int i)
    {
        if (m_textData.Tags.Length > 0)
        {
            if  (m_textData.Tags.Length > m_tagIndex)
                if (m_textData.Tags[m_tagIndex].Pos <= i)
                {
                    int triggeringPos = m_textData.Tags[m_tagIndex].Pos;
                    HandleTag();
                    while (m_tagIndex < m_textData.Tags.Length && m_textData.Tags[m_tagIndex].Pos == triggeringPos)
                        HandleTag();
                }
        }
    }

    public void DoAllTags()
    {
        for (int i = m_tagIndex; i < m_textData.Tags.Length; i++)
        {
            m_tagIndex = i;
            HandleTag();
        }
    }

    public void HandleTag()
    {
        switch (m_textData.Tags[m_tagIndex].TagType)
        {
            case TagType.Speed:
                m_textBox.TypingSpeed = m_textData.Tags[m_tagIndex].Value;
                m_tagIndex++;
                return;

            case TagType.Sound:
                m_audioHandler.HandleSound(m_audios[m_textData.Tags[m_tagIndex].ValueInt]);
                m_tagIndex++;
                return;

            default:
                ErrorLogger.LogError($"Tag type received but no enums matched with the received type. type received: {m_textData.Tags[m_tagIndex].TagType}");
                return;
        }
    }
}
