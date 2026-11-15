using System.Collections.Generic;
using static BattleFunctions;

public class SideState
{
    private SkillData m_skill;
    public SkillData Skill => m_skill;
    private int m_clashTotal;
    public int ClashTotal => m_clashTotal;
    private DiceList m_diceClash = new();
    public DiceList DiceClash => m_diceClash;
    private DiceList m_dicePointsGain = new();
    public DiceList DicePointsGain => m_dicePointsGain;
    private DiceList m_dicePointsCost = new();
    public DiceList DicePointsCost => m_dicePointsCost;

    public SideState(SkillData skill, int clashTotal)
    {
        m_skill = skill;
        m_clashTotal = clashTotal;
    }

    public SideState()
    {
        m_skill = null;
        m_clashTotal = 0;
    }

    public void SetPower(int value) => m_clashTotal = value;

    public DiceList GetDiceList(SkillDiceType type)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(type))
            return null;

        switch (type)
        {
            case SkillDiceType.Clash:
                return m_diceClash;

            case SkillDiceType.Points:
                return m_dicePointsGain;

            case SkillDiceType.Cost:
                return m_dicePointsCost;

            default:
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return null;
        }
    }
}