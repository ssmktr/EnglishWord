using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : UIBasePanel {

    public UILabel LoadingLbl;
    public UIProgressBar ProgressBar;

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

    public void SetProgressBar(float value)
    {
        ProgressBar.value = value;
    }
}
