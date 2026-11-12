using UnityEngine;

[CreateAssetMenu(fileName = "New Log Effect", menuName = "Battle/Effects/LogEffect")]
public class LogEffect : SkillEffect
{
    public override void Execute(ClashContext context, BattleSide owner)
    {
        ErrorLogger.DebugLog($"Log effect fired at {FireTrigger} by {owner}");
    }
}