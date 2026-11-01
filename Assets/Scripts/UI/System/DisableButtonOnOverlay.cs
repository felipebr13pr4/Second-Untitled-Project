using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DisableButtonOnOverlay : MonoBehaviour
{
    private Button m_buttonComponent;

    private void OnEnable() => OverlayWindow.OnOpen += UpdateState;

    private void Start() => m_buttonComponent = GetComponent<Button>();

    private void OnDisable() => OverlayWindow.OnOpen -= UpdateState;

    private void UpdateState(bool isOverlayOpen) => m_buttonComponent.interactable = !isOverlayOpen;
}
