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


        //TestChanging();

        //TestAdvantages();

        //TestFlipCoin();

        //StartCoroutine(SpecialFunctions.DelayMethodSec(TestClash, 2));
        //StartCoroutine(SpecialFunctions.DelayMethodSec(TestFlipCoin, 2));
    }

    [HideInCallstack]
    private void PullTest(int i, BattleSide side, int value)
    {
        ErrorLogger.DebugLog("------------");
        ErrorLogger.DebugLog($"Test number: {i}");
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