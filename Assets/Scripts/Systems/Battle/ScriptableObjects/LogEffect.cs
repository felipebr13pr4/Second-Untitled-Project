using UnityEngine;

[CreateAssetMenu(fileName = "New Log Effect", menuName = "Battle/Effects/Basic Log")]
public class LogEffect : SkillEffect
{
    public override void Execute(ClashContext context, BattleSide owner)
    {
        ErrorLogger.DebugLog($"Log effect fired at {FireTrigger} by {owner}");
    }
}