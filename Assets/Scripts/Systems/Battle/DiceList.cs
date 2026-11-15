using System.Collections.Generic;

public class DiceList
{
    private List<int> m_diceRolled = new();
    public IReadOnlyList<int> DiceRolled => m_diceRolled;
    public int TotalValue => MathFunctions.GetTotalOfArray(m_diceRolled.ToArray());

    public void AddDice(int[] value)
    {
        foreach (int die in value)
            m_diceRolled.Add(die);
    }

    public bool AddDiceAt(int[] value, int pos)
    {
        if (ErrorLogger.CheckForErrorNegativeArray(pos))
            return false;

        if (pos > m_diceRolled.Count)
            return false;
        for (int i = 0; i < value.Length; i++)
            m_diceRolled.Insert(pos + i, value[i]);

        return true;
    }

    public bool RemoveLastDie()
    {
        if (m_diceRolled.Count == 0)
            return false;
        m_diceRolled.RemoveAt(m_diceRolled.Count - 1);

        return true;
    }

    public bool RemoveDiceByValue(int[] value)
    {
        bool foundAValue = false;
        foreach (int die in value)
        {
            if (m_diceRolled.Contains(die))
                foundAValue = true;
            m_diceRolled.Remove(die);
        }

        return foundAValue;
    }

    public bool RemoveDiceAt(int pos)
    {
        if (ErrorLogger.CheckForErrorNegativeArray(pos))
            return false;

        if (pos > m_diceRolled.Count - 1)
            return false;
        m_diceRolled.RemoveAt(pos);

        return true;
    }

    public void OverwriteDice(int[] dice)
    {
        m_diceRolled.Clear();
        foreach (int die in dice)
            m_diceRolled.Add(die);
    }

    public bool SetDiceAt(int value, int pos)
    {
        if (ErrorLogger.CheckForErrorNegativeArray(pos))
            return false;

        if (pos > m_diceRolled.Count - 1)
            return false;
        m_diceRolled[pos] = value;

        return true;
    }
}
