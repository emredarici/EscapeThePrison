using UnityEngine;

public static class DebugToolKit
{
    public static bool IsDebugModeEnabled = true;

    static DebugToolKit()
    {
#if !UNITY_EDITOR
        IsDebugModeEnabled = false;
#endif
    }

    public static void Log(string message)
    {
        if (!IsDebugModeEnabled) return;
        Debug.Log("[LOG]" + message);
    }

    public static void LogWarning(string message)
    {
        if (!IsDebugModeEnabled) return;
        Debug.LogWarning("[WARNING]" + message);
    }

    public static void LogError(string message)
    {
        if (!IsDebugModeEnabled) return;
        Debug.LogError("[ERROR]" + message);
    }
}
