using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeUIStart : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private bool m_hideInEditor;
    [SerializeField] private bool m_smootherTransition;
    [SerializeField] private float m_time = 2;

    void Start()
    {
        if (m_hideInEditor)
        {
            Color color = m_image.color;
            color.a = 1;
            m_image.color = color;
        }
        StartCoroutine(Fade());
    }

    private void OnValidate()
    {
        if (m_hideInEditor)
        {
            Color color = m_image.color;
            color.a = 0;
            m_image.color = color;
        }
    }

    private IEnumerator Fade()
    {
        Color color = m_image.color;
        float elapsed = 0f;
        float duration = m_time;
        if (m_smootherTransition)
            duration *= 2;

        yield return new WaitForSeconds(duration / 20);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed / duration);
            m_image.color = color;
            yield return null;
            if (duration > m_time && m_smootherTransition)
                duration -= m_time / 10;
        }
        color.a = 0f;
        m_image.color = color;
        m_image.enabled = false;
    }
}
