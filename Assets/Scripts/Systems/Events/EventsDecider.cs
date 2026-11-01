using System;
using UnityEngine;

public class EventsDecider : MonoBehaviour
{
    [SerializeField] private FlagPersistence[] m_flags = new FlagPersistence[Enum.GetValues(typeof(EventFlags)).Length];

    private void Start()
    {
        EventFlagsHolder.Configure(m_flags);
        EventFlagsHolder.Reset();
    }

    private void OnValidate()
    {
        FlagPersistence[] tmpPersistence = new FlagPersistence[Enum.GetValues(typeof(EventFlags)).Length];

        int index = 0;
        foreach (EventFlags flag in Enum.GetValues(typeof(EventFlags)))
        {
            tmpPersistence[index] = new FlagPersistence(flag, false);
            index++;
        }

        for (int i = 0; i < tmpPersistence.Length; i++)
        {
            foreach (FlagPersistence oldFlag in m_flags)
            {
                if (tmpPersistence[i].Flag == oldFlag.Flag)
                {
                    tmpPersistence[i] = new FlagPersistence(tmpPersistence[i].Flag, oldFlag.IsPermanent);
                }
            }
        }

        m_flags = tmpPersistence;
    }
}
