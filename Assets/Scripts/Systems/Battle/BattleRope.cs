using System;
using UnityEngine;

[Serializable]
public class BattleRope
{
    [SerializeField] private int m_value;
    public int Value
    {
        get { return m_value; }
        private set
        {
            int oldValue = m_value;
            m_value = value;
            m_value = Mathf.Clamp(m_value, m_min, m_max);

            if (oldValue == m_value)
                return;

            OnValueChange?.Invoke(oldValue, m_value);

            if (m_value == m_min || m_value == m_max)
                OnWinner?.Invoke(m_value == m_min ? BattleSide.Enemy :
                                 m_value == m_max ? BattleSide.Player : BattleSide.None);
        }
    }
    [SerializeField] private int m_min;
    public int Min => m_min;
    [SerializeField] private int m_max;
    public int Max => m_max;
    public bool IsRopePos => m_value > 0;

    public static event Action<int, int> OnValueChange;
    public static event Action<BattleSide> OnWinner;

    public void Set(int value) => Value = value;
    public void Pull(BattleSide side, int value)
    {
        if (side == BattleSide.None)
        {
            ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
            return;
        }
        Value += side == BattleSide.Player ? value : -value;
    }
    public void Multiply(BattleSide side, float value)
    {
        if (side == BattleSide.None)
        {
            ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
            return;
        }
        else if (m_value == 0)
            return;
        else if (value <= 0)
        {
            ErrorLogger.LogError("A value of 0 or below has been received in the battle rope when trying to multiply. Make sure all skills are not 0 or below in their multiplication values.");
            return;
        }
        else if (side == BattleSide.Player)
            ManageMultiplication(IsRopePos, value);
        else
            ManageMultiplication(!IsRopePos, value);
    }
    private void ManageMultiplication(bool shouldMultiply, float value)
    {
        Value = shouldMultiply ? (int)MathF.Round(m_value * value) :
                (int)MathF.Round(m_value / value);
    }

    public float RopePercentage(float minPercentage = 0, float maxPercentage = 1)
    {
        return MathFunctions.GetPercentageInLine(0, IsRopePos ? Max : Min, Value, minPercentage, maxPercentage);
    }

    // Note: at 0 IsLeading(Enemy) is true because IsRopePos uses <.
    public bool IsLeading(BattleSide side)
    {
        bool result = false;

        if (side == BattleSide.Player)
            result = IsRopePos;
        else if (side == BattleSide.Enemy)
            result = !IsRopePos;
        else
            ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);

        return result;
    }

    public BattleRope(BattleConfig config)
    {
        m_min = config.RopeMin;
        m_max = config.RopeMax;
        m_value = Mathf.Clamp(config.RopeValue, m_min, m_max);
    }

    public BattleRope(int value, int min, int max)
    {
        m_min = min;
        m_max = max;
        m_value = Mathf.Clamp(value, m_min, m_max);
    }
}
