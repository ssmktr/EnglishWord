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
        UIRootInit();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Prev();
    }

    void UIRootInit()
    {
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

    // 패널을 연다
    public UIBasePanel Open(string path, params object[] _parameters)
    {
        UIRootInit();

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

            if (panel._ePanelType != ePanelState.None)
            {
                if (panel != null)
                {
                    if (panel._ePanelType == ePanelState.Default)
                        ListUIPanel.Insert(0, panel);
                    else
                        ListUIPanel.Add(panel);
                }
            }
        }
        else
        {
            // 리스트에서 가장 앞으로 옮긴다
            if (panel._ePanelType != ePanelState.None)
            {
                ListUIPanel.Remove(panel);
                ListUIPanel.Insert(0, panel);
            }
        }
        // 파라미터 저장
        panel.parameters = _parameters;

        if (panel._ePanelType == ePanelState.Default)
            _CurUIBasePanel = panel;
        panel.LateInit();

        //for (int i = 0; i < ListUIPanel.Count; ++i)
        //    Debug.Log(ListUIPanel[i].name);
        //Debug.Log("CurPanel : " + _CurUIBasePanel.name);

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
        // 해당 패널 리스트에서 빼기
        for (int i = 0; i < ListUIPanel.Count; ++i)
        {
            if (ListUIPanel[i].name == name)
            {
                ListUIPanel.RemoveAt(i);
                break;
            }
        }
    }

    public void HideEvent(string name)
    {
        // 해당 패널 리스트의 맨뒤로 보내기
        for (int i = 0; i < ListUIPanel.Count; ++i)
        {
            if (ListUIPanel[i].name == name)
            {
                UIBasePanel panel = ListUIPanel[i];
                ListUIPanel.Remove(panel);
                if (panel._ePanelType != ePanelState.None)
                    ListUIPanel.Add(panel);
                break;
            }
        }
    }

    public void Prev()
    {
        //if (ListUIPanel.Count < 2)
        //{
        //    OnPopupToastPanel("숨겨진 패널이 없습니다");
        //    return;
        //}

        UIBasePanel hidePanel = null;
        UIBasePanel showPanel = null;

        int hideIdx = ListUIPanel.FindIndex(panel => panel._ePanelType != ePanelState.Ignore && _CurUIBasePanel.name == panel.name);
        if (hideIdx >= 0 && ListUIPanel.Count > hideIdx)
            hidePanel = ListUIPanel[hideIdx];
        if (hidePanel is MainPanel || hidePanel is TitlePanel)
        {
            OpenPopup(DataMgr.Instance.GetLocal(5), DataMgr.Instance.GetLocal(6), delegate () 
            {
                Application.Quit();

            }, delegate() { }, DataMgr.Instance.GetLocal(3), DataMgr.Instance.GetLocal(4));
            return;
        }

        int showIdx = ListUIPanel.FindIndex(hideIdx + 1, panel => panel._ePanelType != ePanelState.Ignore);
        if (showIdx >= 0 && ListUIPanel.Count > showIdx)
            showPanel = ListUIPanel[showIdx];

        if (hidePanel != null)
        {
            switch (hidePanel.Prev())
            {
                case PrevType.Not:
                    break;

                case PrevType.OnlyHide:
                    {
                        hidePanel.Hide();
                    }
                    break;

                case PrevType.Hide:
                    {
                        if (showPanel != null)
                        {
                            _CurUIBasePanel = showPanel;
                            showPanel.LateInit();
                        }
                        hidePanel.Hide();
                    }
                    break;

                case PrevType.OnlyClose:
                    {
                        hidePanel.Close();
                    }
                    break;

                case PrevType.Close:
                    {
                        if (showPanel != null)
                        {
                            _CurUIBasePanel = showPanel;
                            showPanel.LateInit();
                        }
                        hidePanel.Close();
                    }
                    break;
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

    // UI 카메라 On, Off
    public void SetUICamera(bool on)
    {
        if (_UICamera != null)
            _UICamera.enabled = on;
    }

    // Popup
    public static PopupPanel Popup = null;
    public void OpenPopup(string title, string message, System.Action okCallback, System.Action cancelCallback = null, string okLbl = "", string cancelLbl = "")
    {
        if (Popup != null)
            return;

        UIBasePanel panel = Open("PopupPanel");
        if (panel != null)
        {
            PopupPanel popup = panel.GetComponent<PopupPanel>();
            if (popup != null)
            {
                Popup = popup;
                popup.OpenPopup(title, message, okCallback, cancelCallback, okLbl, cancelLbl);
            }
        }
    }

    // Loading
    public static LoadingPanel LoadingPanel = null;
    public void OpenLoadingPanel(bool on)
    {
        if (LoadingPanel == null)
        {
            UIBasePanel panel = GetPanel("LoadingPanel");
            if (panel == null)
                panel = Open("LoadingPanel");

            if (panel != null)
            {
                LoadingPanel loading = panel.GetComponent<LoadingPanel>();
                if (loading != null)
                {
                    LoadingPanel = loading;
                }
            }
        }

        if (LoadingPanel != null)
        {
            if (on == false)
                LoadingPanel.Close();
            else
                LoadingPanel.LateInit();
        }
    }
}
