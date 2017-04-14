using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr> {

    public bool IsStartApp = false;

    public string NickName = "";
    public int EnterCost = 0;

    public WordData SingGameWordData = null;

    public Dictionary<InGameItemType, int> DicInGameItem = new Dictionary<InGameItemType, int>();

    #region EVENT
    Dictionary<string, System.Action<object>> DicEvent = new Dictionary<string, System.Action<object>>();
    public void AddEvent(string key, System.Action<object> action)
    {
        if (DicEvent != null && !DicEvent.ContainsKey(key))
            DicEvent.Add(key, action);
    }

    public void RemoveEvent(string key)
    {
        if (DicEvent != null && DicEvent.ContainsKey(key))
            DicEvent.Remove(key);
    }

    public void OnEvent(string key, object param)
    {
        if (DicEvent != null && DicEvent.ContainsKey(key))
            DicEvent[key](param);
    }
    #endregion
}
