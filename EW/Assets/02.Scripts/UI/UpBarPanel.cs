using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBarPanel : UIBasePanel {

    public GameObject EnterCostBtn, BackBtn;
    public UILabel TitleLbl, EnterCostNameLbl, NickNameLbl;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            UIMgr.Instance.Prev();
        };

        // 입장 재화 충전 버튼
        UIEventListener.Get(EnterCostBtn).onClick = OnClickEnterCostBtn;
    }

    private void OnEnable()
    {
        if (GameMgr.Instance != null)
        {
            GameMgr.Instance.AddEvent("SetTitle", SetTitle);
            GameMgr.Instance.AddEvent("SetUpData", SetUpData);
        }
    }

    private void OnDisable()
    {
        if (GameMgr.Instance != null)
        {
            GameMgr.Instance.RemoveEvent("SetTitle");
            GameMgr.Instance.RemoveEvent("SetUpData");
        }
    }

    public override void LateInit()
    {
        base.LateInit();

        SetUpData(null);
    }

    // 타이틀명 설정
    public void SetTitle(object param)
    {
        TitleLbl.text = (string)param;

        BackBtn.SetActive(!(UIMgr.Instance.GetCurBasePanel is MainPanel));
    }

    // 업바에 타이틀 빼고 설정
    public void SetUpData(object param)
    {
        NickNameLbl.text = GameMgr.Instance.NickName;
        SetCosts();
    }

    // 입장 재화
    void SetCosts()
    {
        if (GameMgr.Instance.EnterCost > 0)
            EnterCostNameLbl.text = GameMgr.Instance.EnterCost.ToString("N0");
        else
            EnterCostNameLbl.text = "충전하기";
    }

    void OnClickEnterCostBtn(GameObject sender)
    {
        if (GameMgr.Instance.EnterCost > 0)
        {

        }
    }
}
