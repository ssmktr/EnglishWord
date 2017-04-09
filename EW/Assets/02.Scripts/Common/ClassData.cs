using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordData
{
    public int Id = 0;
    public string English = "";
    public string Korean = "";

    public void Set(WordData data)
    {
        if (data == null)
            return;

        Id = data.Id;
        English = data.English;
        Korean = data.Korean;
    }
}
public class LocalData
{
    public int id = 0;
    public string ko = "";
    public string en = "";
}