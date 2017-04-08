using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr> {

    List<UIBasePanel> ListUIPanel = new List<UIBasePanel>();
    UIBasePanel _CurUIBasePanel = null;
    public UIBasePanel GetCurBasePanel { get { return _CurUIBasePanel; } }

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

        // 토스트 메시지 팝업 저장
        if (PopupToast == null && _System != null)
        {
            PopupToast = (GameObject)Instantiate(Resources.Load("UI/PopupToastPanel"));
            if (PopupToast != null)
            {
                PopupToast.transform.parent = _System;
                PopupToast.transform.localPosition = Vector3.zero;
                PopupToast.transform.localScale = Vector3.one;
                PopupToast.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Prev();
    }

    // 패널을 연다
    public UIBasePanel Open(string path, params object[] _parameters)
    {
        UIBasePanel panel = GetPanel(path);
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
            {
                if (panel._ePanelType == ePanelState.Default)
                    ListUIPanel.Insert(0, panel);
                else
                    ListUIPanel.Add(panel);
            }
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
        if (panel._ePanelType == ePanelState.Default)
            _CurUIBasePanel = panel;

        return panel;
    }

    // 패널있는지 체크
    public UIBasePanel GetPanel(string path)
    {
        return ListUIPanel.Find(data => data.name == path);
    }

    // 첫 패널 가져오기
    public UIBasePanel GetFirstPanel { get { return ListUIPanel.Count > 0 ? ListUIPanel[0] : null; } }

    public void CloseEvent(string name)
    {
        // 해당 패널 리스테에서 빼기
        for (int i = 0; i < ListUIPanel.Count; ++i)
        {
            if (ListUIPanel[i].name == name)
            {
                ListUIPanel.RemoveAt(i);
                break;
            }
        }
    }

    public void Prev()
    {
        // 뒤로 가기 못하는 패널 체크
        if (_CurUIBasePanel is MainPanel || _CurUIBasePanel is TitlePanel)
        {
            OnPopupToastPanel("더 이상 뒤로 갈수 없습니다");
            return;
        }

        int curIdx = -1;
        int prevIdx = -1;
        for (int i = 0; i < ListUIPanel.Count; ++i)
        {
            // 현재 패널에 정해지면 들어옴
            if (curIdx >= 0)
            {
                if (ListUIPanel[i]._ePanelType == ePanelState.Default)
                {
                    prevIdx = i;
                    break;
                }
            }
            // 현재 패널 구하기
            else if (curIdx == -1 && _CurUIBasePanel.name == ListUIPanel[i].name)
            {
                curIdx = i;
            }
        }

        // 현재와 이전 인덱스가 리스트 크기와 맞는지 체크
        if (prevIdx >= 0 && curIdx >= 0 && ListUIPanel.Count > curIdx && ListUIPanel.Count > prevIdx)
        {
            // 현재 패널 바로 뒤에 있는 Default 패널을 연다
            _CurUIBasePanel = ListUIPanel[prevIdx];
            _CurUIBasePanel.LateInit();

            // 현재 패널을 숨기고 리스트의 마지막으로 보낸다
            UIBasePanel panel = ListUIPanel[curIdx];
            if (panel != null)
            {
                panel.Hide();
                ListUIPanel.Remove(panel);
                ListUIPanel.Add(panel);
            }
        }
    }

    // 모든 유아이 삭제
    public void ShotDownUI()
    {
        for (int i = 0; i < ListUIPanel.Count;)
        {
            Destroy(ListUIPanel[i].gameObject);
            ListUIPanel.RemoveAt(i);
        }
    }

    // PopupToast
    GameObject PopupToast = null;
    public void OnPopupToastPanel(string message)
    {
        if (PopupToast != null)
            PopupToast.GetComponent<PopupToastPanel>().ViewMessage(message);
    }
}
