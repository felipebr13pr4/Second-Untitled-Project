using System.Collections.Generic;
using UnityEngine;

public class ClashContext
{
    private BattleRope m_rope;
    public BattleRope Rope => m_rope;
    private SkillData m_playerSkill;
    public SkillData PlayerSkill => m_playerSkill;
    private SkillData m_enemySkill;
    public SkillData EnemySkill => m_enemySkill;
    private int m_playerClashTotal;
    public int PlayerClashTotal => m_playerClashTotal;
    private int m_enemyClashTotal;
    public int EnemyClashTotal => m_enemyClashTotal;
    private DiceContext m_diceClash = new();
    public DiceContext DiceClash => m_diceClash;
    private DiceContext m_dicePointsGain = new();
    public DiceContext DicePointsGain => m_dicePointsGain;
    private DiceContext m_dicePointsCost = new();
    public DiceContext DicePointsCost => m_dicePointsCost;
    private BattleTriggerActions m_currentTrigger;
    public BattleTriggerActions CurrentTrigger => m_currentTrigger;

    public class DiceContext
    {
        private List<int> m_playerDiceRolled = new();
        public IReadOnlyList<int> PlayerDiceRolled => m_playerDiceRolled;
        private List<int> m_enemyDiceRolled = new();
        public IReadOnlyList<int> EnemyDiceRolled => m_enemyDiceRolled;
        public int TotalPlayerValue => MathFunctions.GetTotalOfArray(m_playerDiceRolled.ToArray());
        public int TotalEnemyValue => MathFunctions.GetTotalOfArray(m_enemyDiceRolled.ToArray());


        public IReadOnlyList<int> GetDice(BattleSide side)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return null;

            if (side == BattleSide.Player)
                return PlayerDiceRolled;
            else if (side == BattleSide.Enemy)
                return EnemyDiceRolled;

            return null;
        }

        public void AddDice(BattleSide side, int[] value)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return;
            
            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            foreach (int die in value)
                result.Add(die);
            
            SetDice(side, result);
        }

        public bool AddDiceAt(BattleSide side, int[] value, int pos)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side) ||
                ErrorLogger.CheckForErrorNegativeArray(pos))
                return false;

            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            if (pos > result.Count)
                return false;
            for (int i = 0; i < value.Length; i++)
                result.Insert(pos + i, value[i]);
            
            SetDice(side, result);
            return true;
        }

        public bool RemoveLastDie(BattleSide side)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return false;

            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            if (result.Count == 0)
                return false;
            result.RemoveAt(result.Count - 1);

            SetDice(side, result);
            return true;
        }

        public bool RemoveDiceByValue(BattleSide side, int[] value)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return false;

            bool foundAValue = false;
            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            foreach (int die in value)
            {
                if (result.Contains(die))
                    foundAValue = true;
                result.Remove(die);
            }

            SetDice(side, result);
            return foundAValue;
        }

        public bool RemoveDiceAt(BattleSide side, int pos)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side) ||
                ErrorLogger.CheckForErrorNegativeArray(pos))
                return false;

            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            if (pos > result.Count - 1)
                return false;
            result.RemoveAt(pos);
            
            SetDice(side, result);
            return true;
        }

        public void SetDice(BattleSide side, List<int> dice)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return;

            if (side == BattleSide.Player)
                m_playerDiceRolled = dice;
            else if (side == BattleSide.Enemy)
                m_enemyDiceRolled = dice;
        }

        public bool SetDiceAt(BattleSide side, int value, int pos)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side) ||
                ErrorLogger.CheckForErrorNegativeArray(pos))
                return false;

            if (side == BattleSide.Player)
            {
                if (pos > m_playerDiceRolled.Count - 1)
                    return false;
                m_playerDiceRolled[pos] = value;
            }
            else if (side == BattleSide.Enemy)
            {
                if (pos > m_enemyDiceRolled.Count - 1)
                    return false;
                m_enemyDiceRolled[pos] = value;
            }

            return true;
        }
    }

    public ClashContext(BattleRope rope, SkillData playerSkill, SkillData enemySkill, int playerTotal, int enemyTotal)
    {
        m_rope = rope;
        m_playerSkill = playerSkill;
        m_enemySkill = enemySkill;
        m_playerClashTotal = playerTotal;
        m_enemyClashTotal = enemyTotal;
        m_diceClash = new();
        m_dicePointsGain = new();
        m_dicePointsCost = new();
    }

    public void AddPower(BattleSide side, int value) => SetPower(side, value + (side == BattleSide.Player ? m_playerClashTotal : m_enemyClashTotal));

    public void SetPower(BattleSide side, int value)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return;
        else if (side == BattleSide.Player)
            m_playerClashTotal = value;
        else if (side == BattleSide.Enemy)
            m_enemyClashTotal = value;
    }

    public void SetCurrentTrigger(BattleTriggerActions trigger) => m_currentTrigger = trigger;

    public void ForceMaxDie(BattleSide side, SkillDiceType diceType, int dieValueIndex, int dieToReplace)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return;

        switch (diceType)
        {
            case SkillDiceType.Clash:
                if (side == BattleSide.Player)
                    m_diceClash.SetDiceAt(BattleSide.Player, m_playerSkill.GetMaxPossibleDieOutcome(diceType, dieValueIndex), dieToReplace);
                else
                    m_diceClash.SetDiceAt(BattleSide.Enemy, m_enemySkill.GetMaxPossibleDieOutcome(diceType, dieValueIndex), dieToReplace);
                return;

            case SkillDiceType.Points:
                if (side == BattleSide.Player)
                    m_dicePointsGain.SetDiceAt(BattleSide.Player, m_playerSkill.GetMaxPossibleDieOutcome(diceType, dieValueIndex), dieToReplace);
                else
                    m_dicePointsGain.SetDiceAt(BattleSide.Enemy, m_enemySkill.GetMaxPossibleDieOutcome(diceType, dieValueIndex), dieToReplace);
                return;

            case SkillDiceType.Cost:
                if (side == BattleSide.Player)
                    m_dicePointsCost.SetDiceAt(BattleSide.Player, m_playerSkill.GetMaxPossibleDieOutcome(diceType, dieValueIndex), dieToReplace);
                else
                    m_dicePointsCost.SetDiceAt(BattleSide.Enemy, m_enemySkill.GetMaxPossibleDieOutcome(diceType, dieValueIndex), dieToReplace);
                return;

            default:
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return;
        }
    }

    public void MultiplyDie(BattleSide side, SkillDiceType diceType, int dieToReplace, float multiply = 1)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return;

        if (multiply <= 0)
        {
            ErrorLogger.LogError("A multiplication to a die has been received yet it is negative or zero. Ensure all die multiplications are '> 0'.\n"
                + "");
            return;
        }

        switch (diceType)
        {
            case SkillDiceType.Clash:
                if (side == BattleSide.Player)
                    m_diceClash.SetDiceAt(BattleSide.Player, Mathf.RoundToInt(m_diceClash.PlayerDiceRolled[dieToReplace] * multiply), dieToReplace);
                else
                    m_diceClash.SetDiceAt(BattleSide.Enemy, Mathf.RoundToInt(m_diceClash.EnemyDiceRolled[dieToReplace] * multiply), dieToReplace);
                return;

            case SkillDiceType.Points:
                if (side == BattleSide.Player)
                    m_dicePointsGain.SetDiceAt(BattleSide.Player, Mathf.RoundToInt(m_dicePointsGain.PlayerDiceRolled[dieToReplace] * multiply), dieToReplace);
                else
                    m_dicePointsGain.SetDiceAt(BattleSide.Enemy, Mathf.RoundToInt(m_dicePointsGain.EnemyDiceRolled[dieToReplace] * multiply), dieToReplace);
                return;

            case SkillDiceType.Cost:
                if (side == BattleSide.Player)
                    m_dicePointsCost.SetDiceAt(BattleSide.Player, Mathf.RoundToInt(m_dicePointsCost.PlayerDiceRolled[dieToReplace] * multiply), dieToReplace);
                else
                    m_dicePointsCost.SetDiceAt(BattleSide.Enemy, Mathf.RoundToInt(m_dicePointsCost.EnemyDiceRolled[dieToReplace] * multiply), dieToReplace);
                return;

            default:
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return;
        }
    }

    public IReadOnlyList<int> GetDice(BattleSide side, SkillDiceType diceType)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side) ||
            BattleFunctions.Misc.CheckForErrorNone(diceType))
            return null;

        switch (diceType)
        {
            case SkillDiceType.Clash:
                return m_diceClash.GetDice(side);

            case SkillDiceType.Points:
                return m_dicePointsGain.GetDice(side);

            case SkillDiceType.Cost:
                return m_dicePointsCost.GetDice(side);

            default:
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return null;
        }
    }

    public int GetWinnerPointsGain(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return 0;

        if (side == BattleSide.Player)
            return m_dicePointsGain.TotalPlayerValue;
        else if (side == BattleSide.Enemy)
            return m_dicePointsGain.TotalEnemyValue;

        return 0;
    }

    public int GetWinnerPointsCost(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return 0;

        if (side == BattleSide.Player)
            return m_dicePointsCost.TotalPlayerValue;
        else if (side == BattleSide.Enemy)
            return m_dicePointsCost.TotalEnemyValue;

        return 0;
    }

    public SkillData GetSkill(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return new();

        if (side == BattleSide.Player)
            return m_playerSkill;
        else if (side == BattleSide.Enemy)
            return m_enemySkill;

        return new();
    }
}