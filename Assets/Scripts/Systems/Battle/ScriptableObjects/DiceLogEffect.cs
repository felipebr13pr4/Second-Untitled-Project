using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New DiceLog Effect", menuName = "Battle/Effects/DiceLogEffect")]
public class DiceLogEffect : SkillEffect
{
    public override void Execute(ClashContext context, BattleSide owner)
    {
        ErrorLogger.DebugLog($"Log effect fired at {FireTrigger} by {owner}");
        ErrorLogger.DebugLog($"Player clash die results:");
        ErrorLogger.LogArray(context.DiceClash.PlayerDiceRolled.ToArray());
        ErrorLogger.DebugLog($"Enemy clash die results:");
        ErrorLogger.LogArray(context.DiceClash.EnemyDiceRolled.ToArray());
    }
}