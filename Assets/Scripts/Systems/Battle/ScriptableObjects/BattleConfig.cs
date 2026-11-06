using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Config", menuName = "Battle/Config")]
public class BattleConfig : ScriptableObject
{
    [SerializeField] private int m_ropeMin = BattleDefaultValues.RopeMin;
    public int RopeMin => m_ropeMin;
    [SerializeField] private int m_ropeMax = BattleDefaultValues.RopeMax;
    public int RopeMax => m_ropeMax;
    [SerializeField] private int m_ropeValue = BattleDefaultValues.RopeValue;
    public int RopeValue => m_ropeValue;
    [SerializeField] private int m_slotsCounts = BattleDefaultValues.SlotsCounts;
    public int SlotsCounts => m_slotsCounts;
}