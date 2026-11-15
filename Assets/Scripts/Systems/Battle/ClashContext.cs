using System.Collections.Generic;
using UnityEngine;

public class ClashContext
{
    private BattleRope m_rope;
    public BattleRope Rope => m_rope;
    private SideState m_player;
    public SideState Player => m_player;
    private SideState m_enemy;
    public SideState Enemy => m_enemy;
    public SkillData PlayerSkill => m_player.Skill;
    public SkillData EnemySkill => m_enemy.Skill;
    public int PlayerClashTotal => m_player.ClashTotal;
    public int EnemyClashTotal => m_enemy.ClashTotal;
    private BattleTriggerActions m_currentTrigger;
    public BattleTriggerActions CurrentTrigger => m_currentTrigger;

    public ClashContext(BattleRope rope, SkillData playerSkill, SkillData enemySkill, int playerTotal, int enemyTotal)
    {
        m_rope = rope;
        m_player = new(playerSkill, playerTotal);
        m_enemy = new(enemySkill, enemyTotal);
    }

    public void AddPower(BattleSide side, int value) => SetPower(side, value + (side == BattleSide.Player ? m_player.ClashTotal : m_enemy.ClashTotal));

    public void SetPower(BattleSide side, int value)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return;

        GetSide(side).SetPower(value);
    }

    public void SetCurrentTrigger(BattleTriggerActions trigger) => m_currentTrigger = trigger;

    /// <summary>
    /// Note: This can't force max extra dice later added in the skill. Only dice that are already existent in the skill.
    /// </summary>
    public bool ForceMaxDie(BattleSide side, SkillDiceType diceType, int dieToReplace)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side) ||
            BattleFunctions.Misc.CheckForErrorNone(diceType) ||
            ErrorLogger.CheckForErrorNegativeArray(dieToReplace))
            return false;

        DiceList list = GetSide(side).GetDiceList(diceType);

        if (dieToReplace >= GetSkill(side).GetDieCount(diceType)
            ||
            dieToReplace > list.DiceRolled.Count - 1)
            return false;

        list.SetDiceAt(
            GetSide(side).Skill.GetMaxPossibleDieOutcome(diceType, dieToReplace),
            dieToReplace);

        return true;
    }

    public bool MultiplyDie(BattleSide side, SkillDiceType diceType, int dieToReplace, float multiply = 1)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side) ||
            BattleFunctions.Misc.CheckForErrorNone(diceType) ||
            ErrorLogger.CheckForErrorNegativeArray(dieToReplace))
            return false;

        if (multiply <= 0)
        {
            ErrorLogger.LogError("A multiplication to a die has been received yet it is negative or zero. Ensure all die multiplications are '> 0'.\n"
                + "");
            return false;
        }

        DiceList list = GetSide(side).GetDiceList(diceType);

        if (dieToReplace > list.DiceRolled.Count - 1)
            return false;

        list.SetDiceAt(
            Mathf.RoundToInt(
                list.DiceRolled[dieToReplace]
                * multiply),
            dieToReplace);

        return true;
    }

    public IReadOnlyList<int> GetDice(BattleSide side, SkillDiceType diceType)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side) ||
            BattleFunctions.Misc.CheckForErrorNone(diceType))
            return null;

        return GetSide(side).GetDiceList(diceType).DiceRolled;
    }

    public int GetWinnerPointsGain(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return 0;

        return GetSide(side).DicePointsGain.TotalValue;
    }

    public int GetWinnerPointsCost(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return 0;

        return GetSide(side).DicePointsCost.TotalValue;
    }

    public SkillData GetSkill(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return null;

        if (side == BattleSide.Player)
            return m_player.Skill;
        else if (side == BattleSide.Enemy)
            return m_enemy.Skill;

        return null;
    }

    public SideState GetSide(BattleSide side)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return null;

        if (side == BattleSide.Player)
            return m_player;
        else if (side == BattleSide.Enemy)
            return m_enemy;

        return null;
    }
}