using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBaseState : FSM.BaseState<SceneManagerCustom> {

    public string SceneName = "";
    public override void OnEnter(System.Action callback = null)
    {
        base.OnEnter(callback);
    }

    public override void OnExit(System.Action callback = null)
    {
        base.OnExit(callback);
    }

    public void LoadLevelAsync(string _sceneName, System.Action callback = null)
    {
        StartCoroutine(_LoadLevelAsync(_sceneName, callback));
    }

    IEnumerator _LoadLevelAsync(string _sceneName, System.Action callback = null)
    {
        // 로딩 프로그레스바 초기화
        if (UIMgr.LoadingPanel != null)
            UIMgr.LoadingPanel.SetProgressBar(0);

        yield return new WaitForSeconds(0.5f);

        // 같은 씬인지 체크
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == _sceneName)
            GameHelper.DevDebugLog("같은 씬 입니다", LOGSTATE.WARRING);
        else
        {
            Application.backgroundLoadingPriority = ThreadPriority.High;
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName);

            while (!async.isDone)
            {
                // 로딩 프로그레스바 값
                if (UIMgr.LoadingPanel != null)
                    UIMgr.LoadingPanel.SetProgressBar(async.progress);
                yield return null;
            }

            Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
            if (callback != null)
                callback();
        }
    }
}
