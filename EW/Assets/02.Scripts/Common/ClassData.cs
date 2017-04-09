using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordData
{
    public int id = 0;
    public string english = "";
    public string korean = "";

    public void Set(WordData data)
    {
        if (data == null)
            return;

        id = data.id;
        english = data.english;
        korean = data.korean;
    }
}
public class LocalData
{
    public int id = 0;
    public string ko = "";
    public string en = "";
}