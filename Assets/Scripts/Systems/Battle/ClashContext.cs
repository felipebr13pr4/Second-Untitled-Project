using System.Collections.Generic;

public class ClashContext
{
    private BattleRope m_rope;
    public BattleRope Rope => m_rope;
    private SkillData m_playerSkill;
    public SkillData PlayerSkill => m_playerSkill;
    private SkillData m_enemySkill;
    public SkillData EnemySkill => m_enemySkill;
    private int m_playerClashTotal;
    public int PlayerClashTotal => m_playerClashTotal;
    private int m_enemyClashTotal;
    public int EnemyClashTotal => m_enemyClashTotal;
    private DiceContext m_diceClash = new();
    public DiceContext DiceClash => m_diceClash;
    private DiceContext m_dicePointsGain = new();
    public DiceContext DicePointsGain => m_dicePointsGain;
    private DiceContext m_dicePointsCost = new();
    public DiceContext DicePointsCost => m_dicePointsCost;

    public class DiceContext
    {
        private List<int> m_playerDiceRolled = new();
        public IReadOnlyList<int> PlayerDiceRolled => m_playerDiceRolled;
        private List<int> m_enemyDiceRolled = new();
        public IReadOnlyList<int> EnemyDiceRolled => m_enemyDiceRolled;

        public void AddDice(BattleSide side, int[] value)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return;
            
            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            foreach (int die in value)
                result.Add(die);
            
            SetDice(side, result);
        }

        public bool AddDiceAt(BattleSide side, int[] value, int pos)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return false;

            if (pos < 0)
            {
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NegativeArrayEntry);
                return false;
            }

            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            if (pos > result.Count)
                return false;
            for (int i = 0; i < value.Length; i++)
                result.Insert(pos + i, value[i]);
            
            SetDice(side, result);
            return true;
        }

        public bool RemoveLastDie(BattleSide side)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return false;

            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            if (result.Count == 0)
                return false;
            result.RemoveAt(result.Count - 1);

            SetDice(side, result);
            return true;
        }

        public bool RemoveDiceByValue(BattleSide side, int[] value)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return false;

            bool foundAValue = false;
            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            foreach (int die in value)
            {
                if (result.Contains(die))
                    foundAValue = true;
                result.Remove(die);
            }

            SetDice(side, result);
            return foundAValue;
        }

        public bool RemoveDiceAt(BattleSide side, int pos)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return false;

            if (pos < 0)
            {
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NegativeArrayEntry);
                return false;
            }

            List<int> result = new(side == BattleSide.Player ? m_playerDiceRolled : m_enemyDiceRolled);
            if (pos > result.Count - 1)
                return false;
            result.RemoveAt(pos);
            
            SetDice(side, result);
            return true;
        }

        public void SetDice(BattleSide side, List<int> dice)
        {
            if (BattleFunctions.Misc.CheckForErrorNone(side))
                return;

            else if (side == BattleSide.Player)
                m_playerDiceRolled = dice;
            else if (side == BattleSide.Enemy)
                m_enemyDiceRolled = dice;
        }
    }

    public ClashContext(BattleRope rope, SkillData playerSkill, SkillData enemySkill, int playerTotal, int enemyTotal)
    {
        m_rope = rope;
        m_playerSkill = playerSkill;
        m_enemySkill = enemySkill;
        m_playerClashTotal = playerTotal;
        m_enemyClashTotal = enemyTotal;
        m_diceClash = new();
        m_dicePointsGain = new();
        m_dicePointsCost = new();
    }

    public void AddPower(BattleSide side, int value) => SetPower(side, value + (side == BattleSide.Player ? m_playerClashTotal : m_enemyClashTotal));

    public void SetPower(BattleSide side, int value)
    {
        if (BattleFunctions.Misc.CheckForErrorNone(side))
            return;
        else if (side == BattleSide.Player)
            m_playerClashTotal = value;
        else if (side == BattleSide.Enemy)
            m_enemyClashTotal = value;
    }
}
