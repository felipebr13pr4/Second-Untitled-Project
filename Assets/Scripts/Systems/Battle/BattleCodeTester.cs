# if UNITY_EDITOR
using UnityEngine;
using static BattleFunctions.Skill;

public class BattleCodeTester : MonoBehaviour
{
    [SerializeField] private BattleConfig m_config;
    [SerializeField] private BattleRope m_battleRope;
    [SerializeField] private SkillData m_skill1;
    [SerializeField] private SkillData m_skill2;
    [SerializeField] private SkillData m_skill3;
    [SerializeField] private SkillData m_skill4;


    private void OnEnable()
    {
        // BattleRope.OnWinner += TestWin;
    }

    private void OnDisable()
    {
        // BattleRope.OnWinner -= TestWin;
    }

    private void Start()
    {
        m_battleRope = new(m_config);
        ErrorLogger.LogVar(m_battleRope.Value, nameof(m_battleRope.Value));
        ErrorLogger.LogVar(m_battleRope.Min, nameof(m_battleRope.Min));
        ErrorLogger.LogVar(m_battleRope.Max, nameof(m_battleRope.Max));

        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");


        //PullTest(0, BattleSide.Player, 40);
        //TestAverage(1000, m_skill1, m_skill1);

        //TestChanging();

        //TestAdvantages();

        //TestFlipCoin();

        //StartCoroutine(SpecialFunctions.DelayMethodSec(TestClash, 2));
        //StartCoroutine(SpecialFunctions.DelayMethodSec(TestFlipCoin, 2));
    }

    [HideInCallstack]
    private void PullTest(int testNumber, BattleSide side, int value)
    {
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog($"Test number: {testNumber}");
        m_battleRope.Pull(side, value);
        ErrorLogger.LogVar(m_battleRope.Value, nameof(m_battleRope.Value));
        ErrorLogger.DebugLog("------------");
    }

    [HideInCallstack]
    private void MultiplyTest(int i, BattleSide side, float value)
    {
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog($"Test number: {i}");
        m_battleRope.Multiply(side, value);
        ErrorLogger.LogVar(m_battleRope.Value, nameof(m_battleRope.Value));
        ErrorLogger.DebugLog("------------");
    }

    private void TestAverage(int times, SkillData skill1, SkillData skill2)
    {
        int playerWins = 0;
        int enemyWins = 0;
        int ties = 0;
        float allPointsPlayer = 0;
        float allCostsPlayer = 0;
        float allPointsEnemy = 0;
        float allCostsEnemy = 0;

        for (int i = 0; i < times; i++)
        {
            ClashResult result = new();
            result = Clash(skill1, skill2, m_battleRope);

            if (result.Winner == BattleSide.Player)
                playerWins++;
            else if (result.Winner == BattleSide.Enemy)
                enemyWins++;
            else
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);

            if (result.WasTie)
                ties++;

            if (result.Winner == BattleSide.Player)
                allPointsPlayer += result.PointsGain;
            else if (result.Winner == BattleSide.Enemy)
                allPointsEnemy += result.PointsGain;
            else
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);

            if (result.Winner == BattleSide.Player)
                allCostsPlayer += result.PointsCost;
            else if (result.Winner == BattleSide.Enemy)
                allCostsEnemy += result.PointsCost;
            else
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
        }

        ErrorLogger.LogVar(playerWins, nameof(playerWins));
        ErrorLogger.LogVar(enemyWins, nameof(enemyWins));
        ErrorLogger.LogVar(ties, nameof(ties));
        if (playerWins != 0)
        {
            ErrorLogger.LogVar(allPointsPlayer / playerWins, nameof(allPointsPlayer));
            ErrorLogger.LogVar(allCostsPlayer / playerWins, nameof(allCostsPlayer));
        }
        else
        {
            ErrorLogger.DebugLog("No player wins so can't average points gain and cost because of the need of dividing.");
        }
        if (enemyWins != 0)
        {
            ErrorLogger.LogVar(allPointsEnemy / enemyWins, nameof(allPointsEnemy));
            ErrorLogger.LogVar(allCostsEnemy / enemyWins, nameof(allCostsEnemy));
        }
        else
        {
            ErrorLogger.DebugLog("No enemy wins so can't average points gain and cost because of the need of dividing.");
        }
    }

    private void TestAdvantages()
    {
        ErrorLogger.DebugLog($"Has advantage (low Vs Mid)? {HasAdvantage(SkillType.Low, SkillType.Mid)}");
        ErrorLogger.DebugLog($"Has advantage (mid Vs High)? {HasAdvantage(SkillType.Mid, SkillType.High)}");
        ErrorLogger.DebugLog($"Has advantage (high Vs Low)? {HasAdvantage(SkillType.High, SkillType.Low)}");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog($"Has advantage (mid Vs Low)? {HasAdvantage(SkillType.Mid, SkillType.Low)}");
        ErrorLogger.DebugLog($"Has advantage (high Vs Mid)? {HasAdvantage(SkillType.High, SkillType.Mid)}");
        ErrorLogger.DebugLog($"Has advantage (low Vs High)? {HasAdvantage(SkillType.Low, SkillType.High)}");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog($"Has advantage (low Vs Low)? {HasAdvantage(SkillType.Low, SkillType.Low)}");
        ErrorLogger.DebugLog($"Has advantage (mid Vs Mid)? {HasAdvantage(SkillType.Mid, SkillType.Mid)}");
        ErrorLogger.DebugLog($"Has advantage (high Vs High)? {HasAdvantage(SkillType.High, SkillType.High)}");

    }

    private void TestChanging()
    {
        PullTest(2, BattleSide.Player, 30);
        PullTest(3, BattleSide.Player, 35);
        PullTest(4, BattleSide.Enemy, 100);
        MultiplyTest(5, BattleSide.Player, 1.2f);
        MultiplyTest(6, BattleSide.Player, 1.2f);
        MultiplyTest(7, BattleSide.Enemy, 1.3f);
        PullTest(8, BattleSide.Player, 40);
        MultiplyTest(9, BattleSide.Enemy, 1.3f);
        MultiplyTest(10, BattleSide.Player, 1.2f);
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
    }

    private void TestClash()
    {
        ErrorLogger.DebugLog("Initiating first clash test.");
        ErrorLogger.DebugLog("------------");
        Clash(m_skill1, m_skill2, m_battleRope);
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("Initiating second clash test.");
        ErrorLogger.DebugLog("------------");
        Clash(m_skill2, m_skill1, m_battleRope);
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("Initiating third clash test.");
        ErrorLogger.DebugLog("------------");
        Clash(m_skill3, m_skill4, m_battleRope);
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog("Initiating fourth clash test.");
        ErrorLogger.DebugLog("------------");
        Clash(m_skill4, m_skill3, m_battleRope);
    }

    private void TestFlipCoin()
    {
        /*m_battleRope.Set(0);
        FlipCoin(m_battleRope);
        FlipCoin(m_battleRope);
        FlipCoin(m_battleRope);
        m_battleRope.Set(10);
        FlipCoin(m_battleRope);
        FlipCoin(m_battleRope);
        FlipCoin(m_battleRope);
        m_battleRope.Set(-10);
        FlipCoin(m_battleRope);
        FlipCoin(m_battleRope);
        FlipCoin(m_battleRope);*/
    }

    private void TestWin(BattleSide side)
    {
        if (side == BattleSide.Player)
        {
            ErrorLogger.DebugLog("------------");
            ErrorLogger.DebugLog("Player won!");
            ErrorLogger.LogVar(m_battleRope.Value, nameof(m_battleRope.Value));
            ErrorLogger.LogVar(m_battleRope.Min, nameof(m_battleRope.Min));
            ErrorLogger.LogVar(m_battleRope.Max, nameof(m_battleRope.Max));
            ErrorLogger.DebugLog("------------");

        }
        else if (side == BattleSide.Enemy)
        {
            ErrorLogger.DebugLog("------------");
            ErrorLogger.DebugLog("Enemy won!");
            ErrorLogger.LogVar(m_battleRope.Value, nameof(m_battleRope.Value));
            ErrorLogger.LogVar(m_battleRope.Min, nameof(m_battleRope.Min));
            ErrorLogger.LogVar(m_battleRope.Max, nameof(m_battleRope.Max));
            ErrorLogger.DebugLog("------------");
        }
        else
        {
            ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
        }
    }
}
#endif