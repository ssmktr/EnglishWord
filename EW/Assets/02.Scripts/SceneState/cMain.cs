using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMain : MonoBehaviour {

	void Start () {
        // 게임을 처음 시작하면 타이틀 패널 처음이 아니라면 메인패널띄운다
        if (!GameMgr.Instance.IsStartApp)
            UIMgr.Instance.Open("TitlePanel");
        else
        {
            UIMgr.Instance.Open("UpBarPanel");
            UIMgr.Instance.Open("MainPanel");
        }
    }
}
