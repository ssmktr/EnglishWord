using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyPanel : UIBasePanel {

    public GameObject StartBtn;

    public override void Init()
    {
        base.Init();

        StartBtn.transform.FindChild("name").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(2001);

        // 게임 시작
        UIEventListener.Get(StartBtn).onClick = OnClickStartBtn;
    }

    public override void LateInit()
    {
        base.LateInit();

        GameMgr.Instance.OnEvent("SetTitle", DataMgr.Instance.GetLocal(2000));
    }

    // 랜덤하게 문제 선택
    void RandomWordData()
    {
        StartCoroutine(_RandomWordData());
    }

    IEnumerator _RandomWordData()
    {
        // 모든 문제를 풀었는지 체크
        if (DataMgr.Instance.ListClearWordData.Count >= DataMgr.Instance.ListWordData.Count)
        {
            UIMgr.Instance.OnPopupToastPanel(DataMgr.Instance.GetLocal(2002));
            yield break;
        }

        UIMgr.Instance.SetUICamera(false);
        WordData wordData = null;
        while (wordData == null)
        {
            bool bClear = false;
            // 랜덤하게 문제 선택
            int idx = Random.Range(0, DataMgr.Instance.ListWordData.Count);
            for (int i = 0; i < DataMgr.Instance.ListClearWordData.Count; ++i)
            {
                // 이미 클리어 했는지 체크
                if (DataMgr.Instance.ListClearWordData[i].Id == DataMgr.Instance.ListWordData[idx].Id)
                {
                    bClear = true;
                    break;
                }
            }

            // 클리어 하지않았으면 저장
            if (!bClear)
                wordData = DataMgr.Instance.ListWordData[idx];

            yield return null;
        }

        // 인게임에 사용할 데이터 저장
        GameMgr.Instance.SingGameWordData = wordData;
        if (GameMgr.Instance.SingGameWordData != null)
        {
            UIMgr.Instance.ShotDownUI();

            SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_GAME);
        }
        else
            UIMgr.Instance.OnPopupToastPanel(DataMgr.Instance.GetLocal(2003));

        UIMgr.Instance.SetUICamera(true);
    }

    void OnClickStartBtn(GameObject sender)
    {
        // 랜덤하게 문제 선택
        RandomWordData();
    }
}
