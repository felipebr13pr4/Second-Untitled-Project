using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_tmp;
    [SerializeField] private DialogueAudioHandler m_audioHandler;
    [SerializeField] private TagHandler m_tagHandler;
    [SerializeField] private float m_typingSpeed = DefaultTypingSpeed;
    public float TypingSpeed { set => m_typingSpeed = value; }
    public const float DefaultTypingSpeed = 0.03f;
    private Coroutine m_coroutine;
    private bool m_isWriting;
    public bool IsWriting => m_isWriting;
    public static event Action<LineExpression> OnExpression;
    public static event Action OnEndLine;

    // Good to see the sources.
    public void Show(string line)
    { 
        m_tmp.text = line;
    }

    public void Hide()
    {
        OnExpression?.Invoke(new LineExpression());
        m_tmp.text = "";
        gameObject.SetActive(false);
    }

    public void SkipToFullLine()
    {
        StopCoroutine(m_coroutine);
        ResetStats();
        Show(m_tagHandler.TextData.Text);
    }

    public void StartWriting(string text, LineExpression expression, AudioData voice, AudioData[] sounds)
    {
        OnExpression?.Invoke(expression);
        m_tagHandler.Initialize(text, sounds);
        if (isActiveAndEnabled)
            m_coroutine = StartCoroutine(TypeWriter(voice));
    }

    private IEnumerator TypeWriter(AudioData voice)
    {
        m_isWriting = true;
        string tmp = "";
        float elapsed = 0f;
        for (int i = 0; i < m_tagHandler.TextData.Text.Length; i++)
        {
            char c = m_tagHandler.TextData.Text[i];
            m_tagHandler.Check(i);
            while (elapsed < m_typingSpeed)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            tmp += c;
            if (char.IsLetter(c))
                m_audioHandler.HandleVoice(voice);
            Show(tmp);
            elapsed = 0f;
        }
        ResetStats();
    }

    private void ResetStats()
    {
        m_tagHandler.DoAllTags();
        OnEndLine?.Invoke();
        m_isWriting = false;
        m_typingSpeed = DefaultTypingSpeed;
    }
}
