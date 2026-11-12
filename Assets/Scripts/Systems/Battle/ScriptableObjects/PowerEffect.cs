using UnityEngine;

[CreateAssetMenu(fileName = "New Power Effect", menuName = "Battle/Effects/PowerEffect")]
public class PowerEffect : SkillEffect
{
    [Range(0, 1)]
    [SerializeField] private float m_percentageRequirement;
    [SerializeField] private int m_powerBoost;

    public override void Execute(ClashContext context, BattleSide owner)
    {
        if (context.Rope.IsLeading(owner) && context.Rope.RopePercentage() > m_percentageRequirement)
            context.AddPower(owner, m_powerBoost);
        ErrorLogger.DebugLog($"Power effect fired at {FireTrigger} by {owner}, did the condition succeed?: {context.Rope.IsLeading(owner) && context.Rope.RopePercentage() > m_percentageRequirement}");
    }
}