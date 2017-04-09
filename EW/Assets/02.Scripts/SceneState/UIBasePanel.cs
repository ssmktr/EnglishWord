using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePanelState
{
    None,
    Default,
    Ignore,
}

public class UIBasePanel : MonoBehaviour {

    // UI 오픈할때 파라미터 저장
    public object[] parameters;

    // 패널 상태
    public ePanelState _ePanelType = ePanelState.Default;

    public virtual void Init()
    {
    }

    public virtual void LateInit()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        UIMgr.Instance.HideEvent(gameObject.name);
        gameObject.SetActive(false);
    }

    public virtual void Close()
    {
        UIMgr.Instance.CloseEvent(gameObject.name);
        Destroy(gameObject);
    }

    public virtual PrevType Prev()
    {
        return PrevType.Hide;
    }
}
