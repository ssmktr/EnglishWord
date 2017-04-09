using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHUDPanel : UIBasePanel {

    public GameObject ExitBtn;
    public UILabel ProblemLbl, AnswerLbl;
    public GameObject[] KeyGroup;

    string Answer = "";
    List<char> ListInputWord = new List<char>();

    // 종료 정산 중엔 뒤로 가기 안됨
    bool IsEndding = false;

    public override PrevType Prev()
    {
        if (IsEndding)
            return PrevType.Not;

        return base.Prev();
    }

    public override void Init()
    {
        base.Init();

        ExitBtn.transform.FindChild("name").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(3001);

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

        SetProblemLable();

        // 정답 표시 (임시)
        AnswerLbl.text = Answer;
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
        char word = char.Parse(sender.name);
        bool bIn = false;
        for (int i = 0; i < ListInputWord.Count; ++i)
        {
            if (ListInputWord[i] == word)
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
                ListInputWord.Add(word);
                SetProblemLable();
            }
            else
            {
                UIMgr.Instance.OnPopupToastPanel("아쉽네요...");
            }
        }
        else
        {
            UIMgr.Instance.OnPopupToastPanel("이미 클릭 했습니다");
        }
    }

    // 단어를 숨길지 표시 할지 체크
    void SetProblemLable()
    {
        string problem = "";
        for (int i = 0; i < Answer.Length; ++i)
        {
            bool bIn = false;
            for (int j = 0; j < ListInputWord.Count; ++j)
            {
                if (ListInputWord[j] == Answer[i])
                {
                    bIn = true;
                    break;
                }
            }

            if (bIn)
            {
                problem += Answer[i];
            }
            else
            {
                problem += "_";
            }
        }

        ProblemLbl.text = problem;
        if (IsSuccessGame())
        {
            StartCoroutine(_EndGame());
        }
    }

    bool IsSuccessGame()
    {
        if (ProblemLbl.text == Answer)
        {
            return true;
        }

        return false;
    }

    // 클리어 한 단어 추가 하고 메인가기
    IEnumerator _EndGame()
    {
        IsEndding = true;
        UIMgr.Instance.SetUICamera(false);

        bool bIn = false;
        for (int i = 0; i < DataMgr.Instance.ListClearWordData.Count; ++i)
        {
            if (DataMgr.Instance.ListClearWordData[i].english.ToUpper() == Answer)
            {
                bIn = true;
                break;
            }
            yield return null;
        }

        if (!bIn)
        {
            DataMgr.Instance.ListClearWordData.Add(GameMgr.Instance.SingGameWordData);
        }

        IsEndding = false;
        UIMgr.Instance.SetUICamera(true);
        UIMgr.Instance.OpenPopup("결과", "정답 입니다", delegate ()
        {
            if (!IsEndding)
                OnClickExitBtn(null);

        }, null, DataMgr.Instance.GetLocal(3001));
    }
}
