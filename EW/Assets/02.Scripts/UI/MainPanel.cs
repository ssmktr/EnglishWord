using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : UIBasePanel {

    public UILabel TitleLbl;

    public override void Init()
    {
        base.Init();

        TitleLbl.text = "메인";
    }

    public override void LateInit()
    {
        base.LateInit();

    }
}
