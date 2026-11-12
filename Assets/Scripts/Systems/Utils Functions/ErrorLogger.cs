using UnityEngine;

public class ErrorLogger : MonoBehaviour
{
    public enum ErrorType
    {
        NoneType = 0,
        NegativeArrayEntry = 1,
        // Put a error here when it is necessary (and will be repeated)
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

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogArray(float[] array)
    {
        string arrayText = "";
        arrayText += "[";
        foreach (float entry in array)
            arrayText += entry.ToString() + ", ";
        arrayText += "]";
        DebugLog(arrayText);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    [HideInCallstack]
    public static void LogArray(int[] array)
    {
        string arrayText = "";
        arrayText += "[";
        foreach (int entry in array)
            arrayText += entry.ToString() + ", ";
        arrayText += "]";
        DebugLog(arrayText);
    }


    private static void HandleLogError(int errorIndex, string varInfo)
    {

        switch (errorIndex)
        {

            case (int)ErrorType.NoneType:
                Debug.LogError("A type enum has been received yet it is none. Make sure to edit all unedited enums or to find the reason why.\n" +
                    "Extra: " + varInfo +
                    "Received error index: " + errorIndex);
                return;

            case (int)ErrorType.NegativeArrayEntry:
                Debug.LogError("A array entry has been received yet it is negative. Make sure all array entries are not negative." +
                    "Extra: " + varInfo +
                    "Received error index: " + errorIndex);
                return;

            default:
                Debug.LogError("Error index out of bounds, please make sure its within them.\n" +
                    "Received error index: " + errorIndex);
                return;
        }
    }
}
