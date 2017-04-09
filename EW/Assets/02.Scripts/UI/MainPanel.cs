using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : UIBasePanel {

    public GameObject ReadyBtn;

    public override void Init()
    {
        base.Init();

        ReadyBtn.transform.FindChild("name").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(1001);

        UIEventListener.Get(ReadyBtn).onClick = OnClickReadyBtn;
    }

    public override void LateInit()
    {
        base.LateInit();

        GameMgr.Instance.OnEvent("SetTitle", DataMgr.Instance.GetLocal(1000));

    }

    void OnClickReadyBtn(GameObject sender)
    {
        Hide();
        UIMgr.Instance.Open("ReadyPanel");
    }
}
