using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : UIBasePanel {

    public GameObject EnterBtn;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(EnterBtn).onClick = (sender) =>
        {
            LoadJsonData();
        };
    }

    public override void LateInit()
    {
        base.LateInit();

    }

    void LoadJsonData()
    {
        StartCoroutine(_LoadJsonData());
    }

    IEnumerator _LoadJsonData()
    {
        WWW www = new WWW(NetworkMgr.Instance.URL("Jsons/worddata.json"));
        GameHelper.DevDebugLog(www.url);

        if (!www.isDone)
        {
            Debug.Log(www.progress);
        }

        yield return www;

        if (www.isDone)
        {
            if (www.error == null)
            {
                DataMgr.Instance.ListWordData.Clear();
                TinyJSON.Variant variant = TinyJSON.JSON.Load(www.text);
                TinyJSON.JSON.MakeInto<List<WordData>>(variant, out DataMgr.Instance.ListWordData);

                // 게임 시작
                Close();
                UIMgr.Instance.Open("MainPanel");
            }
            else
                GameHelper.DevDebugLog(www.error, LOGSTATE.ERROR);
        }
    }
}
