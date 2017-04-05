using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : Singleton<DataMgr> {

    public List<WordData> ListWordData = new List<WordData>();

    public void AllLoadJSon()
    {
        LoadWordJson();
    }

    void LoadWordJson()
    {
        ListWordData.Clear();
        TextAsset textAsset = (TextAsset)Resources.Load("Jsons/worddata");
        TinyJSON.Variant variant = TinyJSON.JSON.Load(textAsset.text);
        TinyJSON.JSON.MakeInto<List<WordData>>(variant, out ListWordData);
    }
}
