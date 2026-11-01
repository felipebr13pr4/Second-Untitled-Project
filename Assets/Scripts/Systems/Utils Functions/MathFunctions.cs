using UnityEngine;

public static class MathFunctions
{
    // code from last project and technically from Claude. Code modified though.
    // Mostly what i needed help was the math.
    /// <summary>
    /// Returns a random number with weight. skewHigh = higher rolls. !skewHigh = lower rolls.
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
    /// </summary>
    public static int SkewedRandom(int min, int max, float bias, bool skewHigh)
    {
        bias = Mathf.Clamp(bias, 1, 100);
        float r = Mathf.Pow(Random.value, 1f / bias);
        if (!skewHigh) r = 1f - r;
        return Mathf.RoundToInt(Mathf.Lerp(min, max, r));
    }
}
