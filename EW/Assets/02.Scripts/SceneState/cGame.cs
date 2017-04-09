using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cGame : MonoBehaviour {

	void Start () {
        UIMgr.Instance.OpenLoadingPanel(false);
        UIMgr.Instance.Open("InGameHUDPanel");
	}
}
