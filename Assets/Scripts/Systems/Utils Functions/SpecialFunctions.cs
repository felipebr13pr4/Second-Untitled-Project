using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public static class SpecialFunctions
{
    /// <summary>
    /// Remember to put start coroutine. It seems to not warn for some reason.
    /// </summary>
    public static IEnumerator DelayMethodFrame(Action method)
    {
        yield return null;
        method?.Invoke();
    }

    /// <summary>
    /// Remember to put start coroutine. It seems to not warn for some reason.
    /// </summary>
    public static IEnumerator DelayMethodFrame(Action method, int framesAmount)
    {
        for (int i = 0; i < framesAmount; i++)
            yield return null;
        method?.Invoke();
    }

    /// <summary>
    /// Remember to put start coroutine. It seems to not warn for some reason.
    /// </summary>
    public static IEnumerator DelayMethodSec(Action method, float time)
    {
        yield return new WaitForSeconds(time);
        method?.Invoke();
    }

    /// <summary>
    /// Remember to put start coroutine. It seems to not warn for some reason.
    /// </summary>
    public static IEnumerator DelayMethodSecReal(Action method, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        method?.Invoke();
    }
}
