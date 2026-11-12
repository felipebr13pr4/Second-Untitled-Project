public struct DiceResult
{
    private int m_totalValue;
    public readonly int TotalValue => m_totalValue;
    private int[] m_rolledValues;
    public readonly int[] RolledValues => m_rolledValues;

    public DiceResult(int totalValue, int[] rolledValues)
    {
        m_totalValue = totalValue;
        m_rolledValues = rolledValues;
    }

}