using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : SceneBaseState {

    bool NextSceneCheck = false;

    public override void OnEnter(Action callback = null)
    {
        SceneName = "StartScene";
        base.OnEnter(callback);

        LoadLevelAsync(SceneName);
        StartInit();
    }

    public override void OnExit(Action callback = null)
    {
        base.OnExit(callback);
    }

    void StartInit()
    {
        if (!NextSceneCheck)
        {
            NextSceneCheck = true;

            Invoke("NextScene", 0.5f);
        }
    }

    void NextScene()
    {
        SceneManagerCustom.Instance.ActionEvent(_ACTION.GO_MAIN);
    }
}
