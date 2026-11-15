using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dice Repeat Power Effect", menuName = "Battle/Effects/Dice Repeat Power")]
public class DiceRepeatPowerMax : SkillEffect
{
    [SerializeField] private int m_repeatNumber;
    [SerializeField] private int m_repeatAmount;
    [SerializeField] private float m_multiplication;
    [SerializeField] private int m_dieToReplace;
    [SerializeField] private SkillDiceType m_dieTypeToChange;

    public override void Execute(ClashContext context, BattleSide owner)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(owner))
            return;

        int repeats = 0;

        ErrorLogger.DebugLog($"My owner is {owner}.");

        SideState side = context.GetSide(owner);
        IReadOnlyList<int> die = context.GetDice(owner, m_dieTypeToChange);

        for (int i = 0; i < side.DiceClash.DiceRolled.Count; i++)
            if (side.DiceClash.DiceRolled[i] == m_repeatNumber)
                repeats++;

        ErrorLogger.DebugLog($"--{owner} dice");
        ErrorLogger.LogArray(die.ToArray());
        ErrorLogger.DebugLog($"{owner} dice--");

        if (repeats >= m_repeatAmount)
        {
            ErrorLogger.DebugLog($"{owner} got the repeatAmount.");

            if (die.Count != 0)
            {
                ErrorLogger.DebugLog($"--old numbers");
                ErrorLogger.DebugLog($"--{owner} dice");
                ErrorLogger.LogArray(die.ToArray());
                ErrorLogger.DebugLog($"{owner} dice--");
                ErrorLogger.DebugLog($"old numbers--");

                if (m_dieToReplace >= context.GetSkill(owner).GetDieCount(m_dieTypeToChange)
                    ||
                    m_dieToReplace >= die.Count)
                {
                    ErrorLogger.LogError("Index out of bounds in the dice repeat power max, a index to change a die has been received but the die to be changed has an lenght smaller than the number.");
                    return;
                }

                context.ForceMaxDie(owner, m_dieTypeToChange, m_dieToReplace);
                context.MultiplyDie(owner, m_dieTypeToChange, m_dieToReplace, m_multiplication);

                ErrorLogger.DebugLog($"--newly altered numbers.");
                ErrorLogger.DebugLog($"--{owner} dice");
                ErrorLogger.LogArray(die.ToArray());
                ErrorLogger.DebugLog($"{owner} dice--");
                ErrorLogger.DebugLog($"newly altered numbers.--");
            }

        }
    }
}