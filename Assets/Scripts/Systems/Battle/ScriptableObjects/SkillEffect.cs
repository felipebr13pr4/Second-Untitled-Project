using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    [SerializeField] private BattleTriggerActions m_fireTrigger;
    public BattleTriggerActions FireTrigger => m_fireTrigger;

    public abstract void Execute(ClashContext context, BattleSide owner);
}