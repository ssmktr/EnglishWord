using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : UIBasePanel {

    public GameObject ReadyBtn;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(ReadyBtn).onClick = OnClickReadyBtn;
    }

    public override void LateInit()
    {
        base.LateInit();

    }

    void OnClickReadyBtn(GameObject sender)
    {
        Hide();
        UIMgr.Instance.Open("ReadyPanel");
    }
}
