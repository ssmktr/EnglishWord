using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneManager : Singleton<SceneManager> {
    

    public void Init()
    {
        Application.targetFrameRate = GameDefine.GAME_FRAME;

    }
}
