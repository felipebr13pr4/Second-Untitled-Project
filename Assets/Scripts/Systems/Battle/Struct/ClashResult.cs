using System;
using UnityEngine;

[Serializable]
public struct ClashResult
{
    [SerializeField] private BattleSide m_winner;
    public readonly BattleSide Winner => m_winner;
    [SerializeField] private bool m_wasTie;
    public readonly bool WasTie => m_wasTie;
    [SerializeField] private int m_pointsGain;
    public readonly int PointsGain => m_pointsGain;
    [SerializeField] private int m_pointsCost;
    public readonly int PointsCost => m_pointsCost;
    
    public ClashResult(BattleSide winner, bool wasTie, int pointsGain, int pointsCost)
    {
        m_winner = winner;
        m_wasTie = wasTie;
        m_pointsGain = pointsGain;
        m_pointsCost = pointsCost;
    }
}