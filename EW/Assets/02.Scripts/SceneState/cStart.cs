using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cStart : MonoBehaviour {

	void Start () {
        SceneManagerCustom.Instance.Init_FSM();

        LoadJsonData();
    }

    void LoadJsonData()
    {
        StartCoroutine(_LoadJsonData());
    }

    IEnumerator _LoadJsonData()
    {
        WWW www = new WWW("ssmktr.ivyro.net/EnglishWord/Jsons/worddata.json");

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

                // 메인으로 넘어가기
                SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_MAIN);
            }
            else
                GameHelper.DevDebugLog(www.error, LOGSTATE.ERROR);
        }
    }
}
