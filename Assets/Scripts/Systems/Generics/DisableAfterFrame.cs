using System.Collections;
using UnityEngine;

public class DisableAfterFrame : MonoBehaviour
{
    [SerializeField] private bool m_shouldDoIt;

    private void OnEnable() { if (m_shouldDoIt) StartCoroutine(Disable()); }

    private IEnumerator Disable()
    {   yield return null;
        gameObject.SetActive(false); m_shouldDoIt = false; }
}
