using UnityEngine;
using static BattleDefaultValues;
using static MathFunctions;

public static class BattleFunctions
{
    public static class Skill
    {
        public static bool HasAdvantage(SkillType type1, SkillType type2)
        {
            if (type1 == SkillType.None || type2 == SkillType.None)
            {
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return false;
            }
            return ((int)type2 - 1) == (((int)type1 - 1) + 1) % 3;
        }

        public static ClashResult Clash(SkillData playerSkill, SkillData enemySkill, BattleRope rope)
        {
            // Apply specials here.

            int playerResult = RollRopeBiasDice(rope.IsLeading(BattleSide.Player), rope, MinClashBias, MaxClashBias, playerSkill.Dice);
            int enemyResult = RollRopeBiasDice(rope.IsLeading(BattleSide.Enemy), rope, MinClashBias, MaxClashBias, enemySkill.Dice);

            ErrorLogger.DebugLog("------------");
            ErrorLogger.LogVar(playerResult, nameof(playerResult));
            ErrorLogger.LogVar(enemyResult, nameof(enemyResult));
            ErrorLogger.DebugLog("------------");

            playerResult = CheckBoostAdvantage(playerSkill.Type, enemySkill.Type, playerResult);
            enemyResult = CheckBoostAdvantage(enemySkill.Type, playerSkill.Type, enemyResult);

            ErrorLogger.DebugLog("------------");
            ErrorLogger.DebugLog("new results after clashes.");
            ErrorLogger.LogVar(playerResult, nameof(playerResult));
            ErrorLogger.LogVar(enemyResult, nameof(enemyResult));
            ErrorLogger.DebugLog("------------");
            ErrorLogger.DebugLog($"Did player win?: {playerResult > enemyResult}");

            // Apply specials here.

            ClashResult result = new(BattleSide.None, false, 0, 0);

            if (playerResult == enemyResult)
                result = new(FlipCoin(rope), true, 0, 0);
            else
            {
                BattleSide winner = CheckWinner(playerResult, enemyResult);
                result = new(winner, false, 0, 0);
            }

            SkillData winnerSkill = result.Winner == BattleSide.Player ? playerSkill : enemySkill;
            
            int pointsGain = RollRopeBiasDice(
                !rope.IsLeading(result.Winner),
                rope, MinPointsBias, MaxPointsBias, winnerSkill.Points);
            int pointsCost = RollDice(winnerSkill.PointsRequired);

            pointsCost = Mathf.Max(pointsCost, 1);

            result = new(result.Winner, result.WasTie, pointsGain, pointsCost);

            // Apply specials here.

            return result;
        }

        private static int RollDice(params DieData[] dice)
        {
            int result = 0;

            foreach (DieData die in dice)
            {
                result += die.BaseValue + Random.Range(die.RangeValue.Min, die.RangeValue.Max + 1);
                ErrorLogger.DebugLog($"Current result of rolling dies: {result}");
            }
            return result;
        }

        private static int RollBiasDice(float bias, bool skewHigh, params DieData[] dice)
        {
            int result = 0;

            foreach (DieData die in dice)
            {
                result += die.BaseValue + SkewedRandom(die.RangeValue.Min, die.RangeValue.Max, bias, skewHigh);
                ErrorLogger.DebugLog($"Current result of rolling dies: {result}");
            }
            return result;
        }

        private static int RollRopeBiasDice(bool skewHigh, BattleRope rope, float minPercentage = 0, float maxPercentage = 1, params DieData[] dice)
        {
            float bias = 1 + (rope.RopePercentage(minPercentage, maxPercentage) * DiceBiasMultiplication);

            return RollBiasDice(bias, skewHigh, dice);
        }

        private static int CheckBoostAdvantage(SkillType usingSkill, SkillType clashingSkill, float value)
        {
            if (HasAdvantage(usingSkill, clashingSkill))
                return Mathf.FloorToInt(value * AdvantageBoost);
            else
                return (int)value;
        }

        private static BattleSide CheckWinner(int playerResult, int enemyResult)
        {
            return playerResult > enemyResult ? BattleSide.Player : BattleSide.Enemy;
        }

        private static BattleSide FlipCoin(BattleRope rope)
        {
            ErrorLogger.DebugLog("------------");
            float winInt = Random.value;
            
            float playerChance = GetPercentageInLine(rope.Max, rope.Min, rope.Value, MinFlipCoin, MaxFlipCoin);
            ErrorLogger.LogVar(playerChance, nameof(playerChance));

            BattleSide winner = playerChance > winInt ? BattleSide.Player : BattleSide.Enemy;

            ErrorLogger.DebugLog("------------");
            ErrorLogger.LogVar(winInt, nameof(winInt));
            ErrorLogger.LogVar(winner, nameof(winner));
            ErrorLogger.DebugLog("------------");

            return winner;
        }

        
    }
}
