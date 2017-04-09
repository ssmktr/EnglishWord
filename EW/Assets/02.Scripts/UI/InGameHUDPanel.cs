using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHUDPanel : UIBasePanel {

    public GameObject ExitBtn;
    public UILabel ProblemLbl;
    public GameObject[] KeyGroup;

    string Answer = "";
    List<string> ListInputWord = new List<string>();

    public override void Init()
    {
        base.Init();

        ExitBtn.transform.FindChild("name").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(2);

        // 나가기 버튼
        UIEventListener.Get(ExitBtn).onClick = OnClickExitBtn;

        // 키보드 클릭
        for (int i = 0; i < KeyGroup.Length; ++i)
            UIEventListener.Get(KeyGroup[i]).onClick = OnClickKeyBtn;
    }

    public override void LateInit()
    {
        base.LateInit();

        // 키보드 이름 설정
        SetKeyName();

        Answer = "";
        ListInputWord.Clear();
        if (GameMgr.Instance.SingGameWordData != null)
            Answer = GameMgr.Instance.SingGameWordData.english.ToUpper();
        ProblemLbl.text = Answer;
    }

    // 키보드 이름 설정
    void SetKeyName()
    {
        for (int i = 0; i < KeyGroup.Length; ++i)
        {
            KeyGroup[i].transform.FindChild("name").GetComponent<UILabel>().text = KeyGroup[i].name;
        }
    }

    void OnClickExitBtn(GameObject sender)
    {
        UIMgr.Instance.ShotDownUI();

        UIMgr.Instance.OpenLoadingPanel(true);
        SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_MAIN);
    }

    void OnClickKeyBtn(GameObject sender)
    {
        // 이미 누른 리스트에 있는지 체크
        bool bIn = false;
        for (int i = 0; i < ListInputWord.Count; ++i)
        {
            if (ListInputWord[i] == sender.name)
            {
                bIn = true;
                break;
            }
        }

        // 리스트에 없으면 추가
        if (!bIn)
        {
            if (Answer.Contains(sender.name))
            {
                UIMgr.Instance.OnPopupToastPanel("나이스!!");
            }
            else
            {
                UIMgr.Instance.OnPopupToastPanel("아쉽네요...");
            }

            ListInputWord.Add(sender.name);
        }
        else
        {
            UIMgr.Instance.OnPopupToastPanel("이미 클릭 했습니다");
        }
    }
}
