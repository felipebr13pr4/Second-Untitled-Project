using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Battle/Skill")]
public class SkillData : ScriptableObject
{
    [SerializeField] private SkillType m_type;
    public SkillType Type => m_type;
    [SerializeField] private DieData[] m_dies = new DieData[1];
    public DieData[] Dice => m_dies;
    [SerializeField] private DieData m_points;
    public DieData Points => m_points;
    [SerializeField] private DieData m_pointsRequired;
    public DieData PointsRequired => m_pointsRequired;
    [SerializeField] private SkillEffect[] m_effects;
    public SkillEffect[] Effects => m_effects;

    private void OnValidate()
    {
        if (m_dies.Length <= 0)
            m_dies = new DieData[1];
        for (int i = 0; i < m_dies.Length; i++)
        {
            if (m_dies[i].RangeValue.Min != 0 || m_dies[i].RangeValue.Max != 0)
                m_dies[i] = new(0, new(m_dies[i].RangeValue.Min, m_dies[i].RangeValue.Max));

            if (m_dies[i].BaseValue != 0)
                m_dies[i] = new(m_dies[i].BaseValue, new(0, 0));

            CheckMinViolation(ref m_dies[i]);
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
}