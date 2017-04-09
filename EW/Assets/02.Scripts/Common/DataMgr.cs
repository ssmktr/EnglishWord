using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : Singleton<DataMgr> {

    public List<WordData> ListWordData = new List<WordData>();
    public List<WordData> ListClearWordData = new List<WordData>();

    #region WORDDATA
    public WordData GetWordData(int id)
    {
        return ListWordData.Find(data => data.id == id);
    }

    public WordData GetClearWordData(int id)
    {
        return ListClearWordData.Find(data => data.id == id);
    }
    #endregion

    #region LOCAL
    public Dictionary<int, LocalData> DicLocal = new Dictionary<int, LocalData>();
    public string GetLocal(int id)
    {
        if (DicLocal.ContainsKey(id))
        {
            switch (GameDefine.localType)
            {
                case LocalType.Ko:
                    return DicLocal[id].ko;

                case LocalType.En:
                    return DicLocal[id].en;
            }
        }

        return string.Format("{0} 인덱스 데이터가 없습니다", id);
    }
    #endregion
}
