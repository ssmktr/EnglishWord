using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cGame : MonoBehaviour {

	void Start () {
        Debug.Log(GameMgr.Instance.SingGameWordData.Id);
        UIMgr.Instance.Open("InGameHUDPanel");
	}
	
}
