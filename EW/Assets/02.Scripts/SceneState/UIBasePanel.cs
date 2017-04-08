using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePanelType
{
    Default,
    Ignore,
}

public class UIBasePanel : MonoBehaviour {

    // UI 오픈할때 파라미터 저장
    public object[] parameters;

    public virtual void Init()
    {
    }

    public virtual void LateInit()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Close()
    {
        UIMgr.Instance.CloseEvent(gameObject.name);
        Destroy(gameObject);
    }
}
