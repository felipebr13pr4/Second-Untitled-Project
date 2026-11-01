using UnityEngine;

// By Claude.
public static class MenuTransitionLock
{
    private static float s_unlockTime;
    public static bool IsLocked => Time.realtimeSinceStartup < s_unlockTime;
    public static void LockFor(float seconds) => s_unlockTime = Time.realtimeSinceStartup + seconds;
}
//