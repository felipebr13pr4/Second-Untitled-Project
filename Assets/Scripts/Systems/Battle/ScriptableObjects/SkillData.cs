using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Battle/Skill")]
public class SkillData : ScriptableObject
{
    [SerializeField] private SkillType m_type;
    public SkillType Type => m_type;
    [SerializeField] private DieData[] m_dice = new DieData[1];
    public DieData[] Dice => m_dice;
    [SerializeField] private DieData m_points;
    public DieData Points => m_points;
    [SerializeField] private DieData m_pointsRequired;
    public DieData PointsRequired => m_pointsRequired;
    [SerializeField] private SkillEffect[] m_effects;
    public SkillEffect[] Effects => m_effects;

    private void OnValidate()
    {
        if (m_dice.Length <= 0)
            m_dice = new DieData[1];
        for (int i = 0; i < m_dice.Length; i++)
        {
            if (m_dice[i].RangeValue.Min != 0 || m_dice[i].RangeValue.Max != 0)
                m_dice[i] = new(0, new(m_dice[i].RangeValue.Min, m_dice[i].RangeValue.Max));

            if (m_dice[i].BaseValue != 0)
                m_dice[i] = new(m_dice[i].BaseValue, new(0, 0));

            CheckMinViolation(ref m_dice[i]);
        }

        if (m_pointsRequired.BaseValue < 0)
            m_pointsRequired = new(0, new(m_pointsRequired.RangeValue.Min, m_pointsRequired.RangeValue.Max));
        else if (m_pointsRequired.RangeValue.Max < 0)
            m_pointsRequired = new(m_pointsRequired.BaseValue, new(m_pointsRequired.RangeValue.Min, 0));
        else if (m_pointsRequired.RangeValue.Min < 0)
            m_pointsRequired = new(m_pointsRequired.BaseValue, new(0, m_pointsRequired.RangeValue.Max));

        CheckMinViolation(ref m_points);
        CheckMinViolation(ref m_pointsRequired);
    }

    private void CheckMinViolation(ref DieData data)
    {
        if (data.RangeValue.Min > data.RangeValue.Max)
            data = new(data.BaseValue, new(data.RangeValue.Max, data.RangeValue.Max));
    }

    public int GetMaxPossibleClashOutcome()
    {
        int value = 0;

        foreach (DieData die in m_dice)
            value += die.BaseValue + die.RangeValue.Max;
        return value;
    }

    public int GetMaxPossibleDieOutcome(SkillDiceType diceType, int dieIndex = 0)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(diceType) ||
            ErrorLogger.CheckForErrorNegativeArray(dieIndex))
            return 0;

        switch (diceType)
        {
            case SkillDiceType.Clash:
                return m_dice[dieIndex].BaseValue + m_dice[dieIndex].RangeValue.Max;

            case SkillDiceType.Points:
                return m_points.BaseValue + m_points.RangeValue.Max;

            case SkillDiceType.Cost:
                return m_pointsRequired.BaseValue + m_pointsRequired.RangeValue.Max;

            default:
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return 0;
        }
    }

    public int GetDieCount(SkillDiceType diceType)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(diceType))
            return new();

        return diceType == SkillDiceType.Clash ? m_dice.Length : 1;
    }
}