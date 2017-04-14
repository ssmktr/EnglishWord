using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : UIBasePanel {

    public GameObject OkBtn, CancelBtn;
    public UILabel TitleLbl, MessageLbl, OkLbl, CancelLbl;

    System.Action _OkCallBack = null;
    System.Action _CancelCallBack = null;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(OkBtn).onClick = OnClickOkBtn;
        UIEventListener.Get(CancelBtn).onClick = OnClickCancelBtn;
    }

    public override void LateInit()
    {
        base.LateInit();
    }

    public override PrevType Prev()
    {
        return PrevType.Not;
    }

    public void OpenPopup(string title, string message, System.Action okCallback, System.Action cancelCallback = null, string okLbl = "", string cancelLbl = "")
    {
        // 데이터 설정
        TitleLbl.text = title;
        MessageLbl.text = message;
        if (string.IsNullOrEmpty(okLbl))
            OkLbl.text = okLbl;
        else
            OkLbl.text = DataMgr.Instance.GetLocal(3);

        if (string.IsNullOrEmpty(cancelLbl))
            CancelLbl.text = cancelLbl;
        else
            CancelLbl.text = DataMgr.Instance.GetLocal(4);

        _OkCallBack = okCallback;
        _CancelCallBack = cancelCallback;

        // 타이틀 숨김 처리
        TitleLbl.gameObject.SetActive(!string.IsNullOrEmpty(title));

        // 버튼 숨긴 체크
        
        if (_CancelCallBack != null)
        {
            CancelBtn.SetActive(true);
            OkBtn.transform.localPosition = new Vector3(150, -120, 0);
        }
        else
        {
            CancelBtn.SetActive(false);
            OkBtn.transform.localPosition = new Vector3(0, -120, 0);
        }
    }

    void OnClickOkBtn(GameObject sender)
    {
        if (_OkCallBack != null)
            _OkCallBack();

        UIMgr.Popup = null;
        Close();
    }

    void OnClickCancelBtn(GameObject sender)
    {
        SoundMgr.Instance.Play("skill");
        SoundMgr.Instance.BgPlay("Epic Theme");

        if (_CancelCallBack != null)
            _CancelCallBack();

        UIMgr.Popup = null;
        Close();
    }
}
