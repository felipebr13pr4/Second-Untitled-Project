using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadingMenu : MonoBehaviour
{
    private WaitForSecondsRealtime m_fadeTimer = new(0.01f);
    [SerializeField] private CanvasGroup m_canvasGroup;
    private Coroutine m_fadeCoroutine;
    private float m_alpha;
    private float Alpha
    {
        get => m_alpha;
        set
        {
            m_alpha = value;
            m_alpha = Mathf.Clamp01(m_alpha);
            if (m_alpha == 0) gameObject.SetActive(false);
            else if (m_alpha < 1) m_canvasGroup.blocksRaycasts = false;
            else if (m_alpha == 1) m_canvasGroup.blocksRaycasts = true;
        }
    }

    private void OnEnable()
    {
        Enable();
    }

    public void Enable()
    {
        if (m_fadeCoroutine != null) StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(Fade(1));
    }

    public void Disable()
    {
        if (!isActiveAndEnabled) return;
        if (m_fadeCoroutine != null) StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float target)
    {
        while (Alpha != target)
        {
            Alpha = Mathf.MoveTowards(Alpha, target, 0.05f);
            m_canvasGroup.alpha = Alpha;
            yield return m_fadeTimer;
        }
    }
}
