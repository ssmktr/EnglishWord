using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyPanel : UIBasePanel {

    public GameObject StartBtn;
    public GameObject[] InGameItemGroup;

    public override void Init()
    {
        base.Init();

        StartBtn.transform.FindChild("name").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(2001);

        InGameItemInfo();

        // 게임 시작
        UIEventListener.Get(StartBtn).onClick = OnClickStartBtn;

        for (int i = 0; i < InGameItemGroup.Length; ++i)
            UIEventListener.Get(InGameItemGroup[i]).onClick = OnClickInGameItem;
    }

    public override void LateInit()
    {
        base.LateInit();

        GameMgr.Instance.OnEvent("SetTitle", DataMgr.Instance.GetLocal(2000));

        GameMgr.Instance.DicInGameItem.Clear();
        SelectItemCheck();
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
                if (DataMgr.Instance.ListClearWordData[i].id == DataMgr.Instance.ListWordData[idx].id)
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

            UIMgr.Instance.OpenLoadingPanel(true);
            SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_GAME);
        }
        else
            UIMgr.Instance.OnPopupToastPanel(DataMgr.Instance.GetLocal(2003));

        UIMgr.Instance.SetUICamera(true);
    }

    // 아이템 이름, 가격 표시
    void InGameItemInfo()
    {
        InGameItemGroup[(int)InGameItemType.BonusTimer].transform.FindChild("CostNameLbl").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(2004);
        InGameItemGroup[(int)InGameItemType.BonusChance].transform.FindChild("CostNameLbl").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(2005);
        InGameItemGroup[(int)InGameItemType.HintAlphabet].transform.FindChild("CostNameLbl").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(2006);

        InGameItemGroup[(int)InGameItemType.BonusTimer].transform.FindChild("CostValueLbl").GetComponent<UILabel>().text = "1000";
        InGameItemGroup[(int)InGameItemType.BonusChance].transform.FindChild("CostValueLbl").GetComponent<UILabel>().text = "1500";
        InGameItemGroup[(int)InGameItemType.HintAlphabet].transform.FindChild("CostValueLbl").GetComponent<UILabel>().text = "2000";
    }

    // 선택한 아이템 체크
    void SelectItemCheck()
    {
        InGameItemGroup[(int)InGameItemType.BonusTimer].transform.FindChild("Check").gameObject.SetActive(GameMgr.Instance.DicInGameItem.ContainsKey(InGameItemType.BonusTimer));
        InGameItemGroup[(int)InGameItemType.BonusChance].transform.FindChild("Check").gameObject.SetActive(GameMgr.Instance.DicInGameItem.ContainsKey(InGameItemType.BonusChance));
        InGameItemGroup[(int)InGameItemType.HintAlphabet].transform.FindChild("Check").gameObject.SetActive(GameMgr.Instance.DicInGameItem.ContainsKey(InGameItemType.HintAlphabet));
    }

    #region ONCLICK
    void OnClickStartBtn(GameObject sender)
    {
        // 랜덤하게 문제 선택
        RandomWordData();
    }

    void OnClickInGameItem(GameObject sender)
    {
        InGameItemType type = (InGameItemType)System.Enum.Parse(typeof(InGameItemType), sender.name);

        if (GameMgr.Instance.DicInGameItem.ContainsKey(type))
            GameMgr.Instance.DicInGameItem.Remove(type);
        else
            GameMgr.Instance.DicInGameItem.Add(type, 1);

        SelectItemCheck();

    }
    #endregion
}
