using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHUDPanel : UIBasePanel {

    public GameObject ExitBtn;

    public override void Init()
    {
        base.Init();

        // 나가기 버튼
        UIEventListener.Get(ExitBtn).onClick = OnClickExitBtn;
    }

    public override void LateInit()
    {
        base.LateInit();
    }

    void OnClickExitBtn(GameObject sender)
    {
        UIMgr.Instance.ShotDownUI();

        SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_MAIN);
    }
}
