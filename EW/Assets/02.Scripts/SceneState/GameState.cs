using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : SceneBaseState
{
    public override void OnEnter(Action callback = null)
    {
        SceneName = "GameScene";
        base.OnEnter(callback);

        LoadLevelAsync(SceneName);
    }

    public override void OnExit(Action callback = null)
    {
        base.OnExit(callback);
    }
}
