using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr> {

    List<UIBasePanel> ListUIPanel = new List<UIBasePanel>();
    UIBasePanel _CurUIBasePanel = null;

    UICamera _UICamera = null;
    Transform _Default, _System;

    private void Awake()
    {
        ListUIPanel.Clear();

        GameObject UIRoot = GameObject.Find("UIRoot");
        if (UIRoot == null)
        {
            UIRoot = (GameObject)Instantiate(Resources.Load("UI/UIRoot"));
            UIRoot.name = "UIRoot";
        }

        if (UIRoot != null)
        {
            _UICamera = UIRoot.transform.FindChild("UICamera").GetComponent<UICamera>();
            _Default = _UICamera.transform.FindChild("Default");
            _System = _UICamera.transform.FindChild("System");
        }
    }

    public UIBasePanel Open(string path, params object[] _parameters)
    {
        UIBasePanel panel = GetUI(path);
        if (panel == null)
        {
            GameObject oPanel = (GameObject)Instantiate(Resources.Load("UI/" + path));
            oPanel.name = path;
            oPanel.transform.parent = _Default;
            oPanel.transform.localPosition = Vector3.zero;
            oPanel.transform.localScale = Vector3.one;

            if (oPanel != null)
            {
                panel = oPanel.GetComponent<UIBasePanel>();
                panel.Init();
            }

            if (panel != null)
                ListUIPanel.Insert(0, panel);
        }
        else
        {
            // 리스트에서 가장 앞으로 옮긴다
            ListUIPanel.Remove(panel);
            ListUIPanel.Insert(0, panel);
        }
        // 파라미터 저장
        panel.parameters = _parameters;

        panel.LateInit();
        _CurUIBasePanel = panel;

        return panel;
    }

    public UIBasePanel GetUI(string path)
    {
        return ListUIPanel.Find(data => data.name == path);
    }

    public void CloseEvent(string name)
    {
        for (int i = 0; i < ListUIPanel.Count; ++i)
        {
            if (ListUIPanel[i].name == name)
            {
                _CurUIBasePanel = ListUIPanel[i + 1];
                ListUIPanel.RemoveAt(i);
                break;
            }
        }
    }
}
