using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : MonoBehaviour{

    // 개발 일때만 로그 남기기
    public static void DevDebugLog(object _msg, LOGSTATE _logState = LOGSTATE.NORMAL)
    {
        if (GameDefine.IsDevMode)
        {
            switch (_logState)
            {
                case LOGSTATE.WARRING:
                    Debug.LogWarning(_msg);
                    break;

                case LOGSTATE.ERROR:
                    Debug.LogError(_msg);
                    break;

                default:
                    Debug.Log(_msg);
                    break;
            }
        }
    }
}

