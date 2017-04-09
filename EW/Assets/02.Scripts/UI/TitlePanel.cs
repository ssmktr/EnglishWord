using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : UIBasePanel {

    public GameObject EnterBtn;
    public UIProgressBar ProgressBar;
    bool IsLoadingComplete = false;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(EnterBtn).onClick = (sender) =>
        {
            // 로딩 완료 체크
            if (IsLoadingComplete)
            {
                // 게임 시작
                Close();
                UIMgr.Instance.Open("UpBarPanel");
                UIMgr.Instance.Open("MainPanel");
            }
        };
    }

    public override void LateInit()
    {
        base.LateInit();

        // 게임 실행 했음
        GameMgr.Instance.IsStartApp = true;
        IsLoadingComplete = false;
        EnterBtn.SetActive(IsLoadingComplete);
        ProgressBar.gameObject.SetActive(!IsLoadingComplete);

        LoadJsonData();
    }

    void LoadJsonData()
    {
        ProgressBar.value = 0;
        StartCoroutine(_LoadJsonData());
    }

    IEnumerator _LoadJsonData()
    {
        yield return StartCoroutine(_LoadLocalData());
        yield return StartCoroutine(_LoadWordData());

        EnterBtn.transform.FindChild("name").GetComponent<UILabel>().text = DataMgr.Instance.GetLocal(1);
        IsLoadingComplete = true;
        EnterBtn.SetActive(IsLoadingComplete);
        ProgressBar.gameObject.SetActive(!IsLoadingComplete);
    }

    // 단어 데이터 파싱
    IEnumerator _LoadWordData()
    {
        WWW www = new WWW(NetworkMgr.Instance.URL("Jsons/worddata.json"));
        GameHelper.DevDebugLog(www.url);

        while (!www.isDone)
        {
            ProgressBar.value = www.progress;
            yield return null;
        }

        yield return www;

        if (www.isDone)
        {
            if (www.error == null)
            {
                DataMgr.Instance.ListWordData.Clear();
                TinyJSON.Variant variant = TinyJSON.JSON.Load(www.text);
                TinyJSON.JSON.MakeInto<List<WordData>>(variant, out DataMgr.Instance.ListWordData);
            }
            else
                GameHelper.DevDebugLog(www.error, LOGSTATE.ERROR);
        }
    }

    // 로케일 파싱
    IEnumerator _LoadLocalData()
    {
        WWW www = new WWW(NetworkMgr.Instance.URL("Jsons/localdata.json"));
        GameHelper.DevDebugLog(www.url);

        while (!www.isDone)
        {
            ProgressBar.value = www.progress;
            yield return null;
        }

        yield return www;

        if (www.isDone)
        {
            if (www.error == null)
            {
                DataMgr.Instance.DicLocal.Clear();
                TinyJSON.Variant variant = TinyJSON.JSON.Load(www.text);
                List<LocalData> list = new List<LocalData>();
                TinyJSON.JSON.MakeInto<List<LocalData>>(variant, out list);
                for (int i = 0; i < list.Count; ++i)
                    DataMgr.Instance.DicLocal.Add(list[i].id, list[i]);
            }
            else
                GameHelper.DevDebugLog(www.error, LOGSTATE.ERROR);
        }
    }
}
