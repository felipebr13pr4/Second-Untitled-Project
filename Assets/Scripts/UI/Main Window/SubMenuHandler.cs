using System;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_returnButton;
    [SerializeField] private GameObject m_mainMenu;
    private readonly List<GameObject> m_activeMenus = new(5);
    public List<GameObject> ActiveMenus => m_activeMenus;

    private void OnEnable()
    {
        SubMenu.IsSubMenuActive += UpdateStatus;
    }

    private void OnDisable()
    {
        SubMenu.IsSubMenuActive -= UpdateStatus;
    }

    private void UpdateStatus(bool isIt, GameObject subMenu)
    {
        if (isIt)
        {
            m_activeMenus.Add(subMenu);
        }
        else
        {
            if (m_activeMenus.Contains(subMenu)) m_activeMenus.Remove(subMenu);
        }
        if (m_activeMenus.Count == 0)
        {
            m_returnButton.GetComponent<FadingMenu>().Disable();
        }
    }
}
