using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cStart : MonoBehaviour {

	void Start () {
        SceneManagerCustom.Instance.Init_FSM();
        SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_MAIN);
	}
	
}
