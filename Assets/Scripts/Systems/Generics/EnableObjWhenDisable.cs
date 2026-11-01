using UnityEngine;

public class EnableObjWhenDisable : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObj;

    private void OnDisable() => m_gameObj.SetActive(true);
}
