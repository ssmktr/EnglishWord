using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : SceneBaseState
{
    public override void OnEnter(Action callback = null)
    {
        SceneName = "Game01";
        base.OnEnter(callback);

        LoadLevelAsync(SceneName);
    }

    //private void OnLevelWasLoaded(int level)
    //{

    //}

    public override void OnExit(Action callback = null)
    {
        base.OnExit(callback);
    }
}
