using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : UIBasePanel {

    public UILabel LoadingLbl;

    public override void Init()
    {
        base.Init();

        LoadingLbl.text = "Loading...";
    }

    public override void LateInit()
    {
        base.LateInit();
    }

    public override PrevType Prev()
    {
        return PrevType.Not;
    }

}
