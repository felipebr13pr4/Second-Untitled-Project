using UnityEngine;

public static class MathFunctions
{
    // code from last project and technically from Claude. Code modified though.
    // Mostly what i needed help was the math.

    /// <summary>
    /// Returns a random number with weight. skewHigh = higher rolls. !skewHigh = lower rolls.
    /// <para>(Exponential increase)</para>
    /// </summary>
    public static float SkewedRandom(float min, float max, float bias, bool skewHigh)
    {
        bias = Mathf.Clamp(bias, 1, 100);
        float r = Mathf.Pow(Random.value, 1f / bias);
        if (!skewHigh) r = 1f - r;
        return Mathf.Lerp(min, max, r);
    }

    /// <summary>
    /// Returns a random number with weight. skewHigh = higher rolls. !skewHigh = lower rolls.
    /// <para>(Exponential increase)</para>
    /// </summary>
    public static int SkewedRandom(int min, int max, float bias, bool skewHigh)
    {
        bias = Mathf.Clamp(bias, 1, 100);
        float r = Mathf.Pow(Random.value, 1f / bias);
        if (!skewHigh) r = 1f - r;
        return Mathf.RoundToInt(Mathf.Lerp(min, max, r));
    }

    /// <summary>
    /// Returns a percentage (0-1) of how far a value is in two floats.
    /// <para>Example: 0 minLine (the minimun range) 10 maxLine(the maximun range) and 5 as value (where the point is) would yield 0.5, since 5 is 50% of the way from 0 to 10.</para>
    /// <para>Second Example: Same first parameters as before, but setting minResult as 0.6 and maxResult as 0.8 yields 0.6, since now the minimun is clamped to be atleast 0.6 (60%) of the way there, alongside not being able to go over 0.8 (80%) so setting the value as 9 would yield 0.8 despite 9 being 90% of the way from 0 to 10. </para>
    /// <para>Do note that you can invert the minLine and maxLine for the inverse. Using 10 min and 0 max and a value of 3 would yield 0.7 (70%) while a value of 9 would yield 0.1 (10%)</para>
    /// </summary>
    public static float GetPercentageInLine(float minLine, float maxLine, float value, float minResult = 0, float maxResult = 1)
    {
        return Mathf.Clamp(Mathf.InverseLerp(minLine, maxLine, value), minResult, maxResult);
    }
}
