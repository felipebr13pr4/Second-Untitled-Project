using System.Collections.Generic;
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

        private static void FireEffects(BattleTriggerActions trigger, ClashContext context)
        {
            SearchForEffect(trigger, context, BattleSide.Player);
            SearchForEffect(trigger, context, BattleSide.Enemy);
        }

        private static void SearchForEffect(BattleTriggerActions trigger, ClashContext context, BattleSide side)
        {
            if (Misc.CheckForErrorNone(side))
                return;
            SkillEffect[] effects = side == BattleSide.Player ? context.PlayerSkill.Effects : context.EnemySkill.Effects;
            foreach (SkillEffect effect in effects)
            {
                if (effect == null)
                {
                    ErrorLogger.LogError("An effect has been received without having any effects assigned to it. Make sure to insert a effect in it or to remove it.");
                    continue;
                }
                if (Misc.CheckForErrorNone(effect.FireTrigger))
                    continue;
                if (effect.FireTrigger == trigger)
                    effect.Execute(context, side);
            }
        }

        public static ClashResult Clash(SkillData playerSkill, SkillData enemySkill, BattleRope rope)
        {
            ClashContext context = new(rope, playerSkill, enemySkill, 0, 0);

            FireEffects(BattleTriggerActions.Use, context);

            DiceResult playerClashDie = RollRopeBiasDice(rope.IsLeading(BattleSide.Player), rope, MinClashBias, MaxClashBias, playerSkill.Dice);
            DiceResult enemyClashDie = RollRopeBiasDice(rope.IsLeading(BattleSide.Enemy), rope, MinClashBias, MaxClashBias, enemySkill.Dice);

            context.AddPower(BattleSide.Player, playerClashDie.TotalValue);
            context.AddPower(BattleSide.Enemy, enemyClashDie.TotalValue);
            context.DiceClash.AddDice(BattleSide.Player, playerClashDie.RolledValues);
            context.DiceClash.AddDice(BattleSide.Enemy, enemyClashDie.RolledValues);

            FireEffects(BattleTriggerActions.AfterClashRoll, context);

            ErrorLogger.DebugLog("------------");
            ErrorLogger.LogVar(context.PlayerClashTotal, nameof(context.PlayerClashTotal));
            ErrorLogger.LogVar(context.EnemyClashTotal, nameof(context.EnemyClashTotal));
            ErrorLogger.DebugLog("------------");

            context.SetPower(BattleSide.Player, CheckBoostAdvantage(playerSkill.Type, enemySkill.Type, context.PlayerClashTotal));
            context.SetPower(BattleSide.Enemy, CheckBoostAdvantage(enemySkill.Type, playerSkill.Type, context.EnemyClashTotal));

            FireEffects(BattleTriggerActions.AfterAdvantages, context);

            ErrorLogger.DebugLog("------------");
            ErrorLogger.DebugLog("new results after advantages.");
            ErrorLogger.LogVar(context.PlayerClashTotal, nameof(context.PlayerClashTotal));
            ErrorLogger.LogVar(context.EnemyClashTotal, nameof(context.EnemyClashTotal));
            ErrorLogger.DebugLog("------------");
            ErrorLogger.DebugLog($"Did player win?: {context.PlayerClashTotal > context.EnemyClashTotal}");

            ClashResult result = new(BattleSide.None, false, 0, 0);

            if (context.PlayerClashTotal == context.EnemyClashTotal)
            {
                FireEffects(BattleTriggerActions.ClashTie, context);
                result = new(FlipCoin(context.Rope), true, 0, 0);
            }
            else
            {
                BattleSide winner = CheckWinner(context.PlayerClashTotal, context.EnemyClashTotal);
                result = new(winner, false, 0, 0);
            }

            if (Misc.CheckForErrorNone(result.Winner))
                return new();

            SkillData winnerSkill = result.Winner == BattleSide.Player ? playerSkill : enemySkill;
            SearchForEffect(BattleTriggerActions.ClashWin, context, result.Winner);
            SearchForEffect(BattleTriggerActions.ClashLose, context, result.Winner == BattleSide.Player ? BattleSide.Enemy : BattleSide.Player);

            DiceResult pointsGainDie = RollRopeBiasDice(
                !context.Rope.IsLeading(result.Winner),
                context.Rope, MinPointsBias, MaxPointsBias, winnerSkill.Points);
            DiceResult pointsCostDie = RollDice(winnerSkill.PointsRequired);

            context.DicePointsGain.AddDice(result.Winner, pointsGainDie.RolledValues);
            context.DicePointsCost.AddDice(result.Winner, pointsCostDie.RolledValues);

            pointsCostDie = new(Mathf.Max(pointsCostDie.TotalValue, 1), pointsCostDie.RolledValues);

            result = new(result.Winner, result.WasTie, pointsGainDie.TotalValue, pointsCostDie.TotalValue);

            FireEffects(BattleTriggerActions.SkillEnd, context);

            return result;
        }

        private static DiceResult RollDice(params DieData[] dice)
        {
            List<int> result = new();

            foreach (DieData die in dice)
                result.Add(die.BaseValue + Random.Range(die.RangeValue.Min, die.RangeValue.Max + 1));
            
            return new(GetTotalOfArray(result.ToArray()), result.ToArray());
        }

        private static DiceResult RollBiasDice(float bias, bool skewHigh, params DieData[] dice)
        {
            List<int> result = new();

            foreach (DieData die in dice)
                result.Add(die.BaseValue + SkewedRandom(die.RangeValue.Min, die.RangeValue.Max, bias, skewHigh));
            
            return new(GetTotalOfArray(result.ToArray()), result.ToArray());
        }

        private static DiceResult RollRopeBiasDice(bool skewHigh, BattleRope rope, float minPercentage = 0, float maxPercentage = 1, params DieData[] dice)
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
    public static class Misc
    {
        public static bool CheckForErrorNone(BattleSide side)
        {
            if (side == BattleSide.None)
            {
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return true;
            }
            return false;
        }
        public static bool CheckForErrorNone(BattleTriggerActions trigger)
        {
            if (trigger == BattleTriggerActions.None)
            {
                ErrorLogger.LogErrorType(ErrorLogger.ErrorType.NoneType);
                return true;
            }
            return false;
        }
    }
}
