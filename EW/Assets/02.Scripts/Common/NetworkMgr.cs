using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMgr : Singleton<NetworkMgr> {

    const string _URL = "http://ssmktr.ivyro.net/EnglishWord/";
    public string URL(string file)
    {
        return _URL + file;
    }


}
