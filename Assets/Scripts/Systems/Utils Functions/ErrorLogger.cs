using UnityEngine;

public class ErrorLogger : MonoBehaviour
{
    public enum ErrorType
    {
        // Put a error here when it is necessary
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void DebugLog(object msg)
    {
        Debug.Log(msg);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogVar(object value, string name)
    {
        Debug.Log($"{name}: {value}");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogError(object msg)
    {
        Debug.LogError(msg);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogWarning(object msg)
    {
        Debug.LogWarning(msg);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogErrorType(int errorIndex, string varInfo = "")
    {
        HandleLogError(errorIndex, varInfo);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogErrorType(ErrorType errorType, string varInfo = "")
    {
        HandleLogError((int)errorType, varInfo);
    }

    private static void HandleLogError(int errorIndex, string varInfo)
    {

        switch (errorIndex)
        {

            default:
                Debug.LogError("Error index out of bounds, please make sure its within them.\n" +
                    "Received error index: " + errorIndex);
                return;
        }
    }
}
