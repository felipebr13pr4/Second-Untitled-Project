using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(FadingMenu))]
public class SubMenu : MonoBehaviour
{
    public static event Action<bool, GameObject> IsSubMenuActive;
    private FadingMenu m_fade;
    private bool m_firstTime = true;
    private bool m_isLocked = true;

    private void OnEnable()
    {
        IsSubMenuActive?.Invoke(true, gameObject);
        StartCoroutine(LockState(false));
    }
    private void OnDisable()
    {
        IsSubMenuActive?.Invoke(false, gameObject);
        m_isLocked = true;
    }

    private void Start()
    {
        m_fade = GetComponent<FadingMenu>(); if (m_firstTime)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            m_firstTime = false;
        }
    }

    private IEnumerator LockState(bool isLocked)
    {
        yield return null;
        yield return null;
        m_isLocked = isLocked;
    } 
}