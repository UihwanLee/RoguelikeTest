using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugExtension
{
    /// <summary>
    /// UNITY_EDITOR에서만 Debug 실행하는 확장 메서드
    /// </summary>
    /// <param name="debug"></param>
    /// <param name="message"></param>
    public static void EditorLog(this string message)
    {
# if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
