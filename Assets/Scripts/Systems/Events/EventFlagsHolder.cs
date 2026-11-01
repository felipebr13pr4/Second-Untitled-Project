using System;
using System.Collections.Generic;

public static class EventFlagsHolder
{
    private static readonly Dictionary<EventFlags, bool> s_flags = new();
    private static readonly HashSet<EventFlags> s_flagsSet = new();
    public static event Action OnFlagsChanged;

    static EventFlagsHolder()
    {
        foreach (EventFlags flag in Enum.GetValues(typeof(EventFlags)))
            s_flags[flag] = false;
    }

    public static bool Get(EventFlags flag) => s_flags.TryGetValue(flag, out bool value) && value;
    public static void Set(EventFlags flag, bool value)
    {
        s_flags[flag] = value;
        OnFlagsChanged?.Invoke();
    }

    public static void Configure(FlagPersistence[] flags)
    {
        s_flagsSet.Clear();
        foreach (FlagPersistence flag in flags)
        {
            if (flag.IsPermanent)
                s_flagsSet.Add(flag.Flag);
        }
    }

    public static void Reset()
    {
        foreach (EventFlags flag in Enum.GetValues(typeof(EventFlags)))
            if (!s_flagsSet.Contains(flag))
                s_flags[flag] = false;
    }
}