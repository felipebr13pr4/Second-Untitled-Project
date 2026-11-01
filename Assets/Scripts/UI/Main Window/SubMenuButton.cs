

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SubMenuButton : MonoBehaviour
{
    private Button m_pauseButton;

    private void Awake() => m_pauseButton = GetComponent<Button>();

    private void OnEnable() => m_pauseButton.onClick.AddListener(Action);

    private void OnDisable() => m_pauseButton.onClick.RemoveListener(Action);

    private void Action() => MenuTransitionLock.LockFor(OverlayWindow.m_waitTime);
}