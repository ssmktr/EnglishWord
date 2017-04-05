using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _STATE
{
    START = 0,
    MAIN,
    GAME,
}

public enum _ACTION
{
    GO_START,
    GO_MAIN,
    GO_GAME,
}

public class SceneManagerCustom : Singleton<SceneManagerCustom>
{
    FSM.FSM<_ACTION, _STATE, SceneManagerCustom> FSM = null;

    public void Init_FSM()
    {
        FSM = new FSM.FSM<_ACTION, _STATE, SceneManagerCustom>(this);

        InitFSM_ForClient();
    }

    void InitFSM_ForClient()
    {
        FSM.AddState(_STATE.START, gameObject.AddComponent<StartState>());
        FSM.AddState(_STATE.MAIN, gameObject.AddComponent<MainState>());
        FSM.AddState(_STATE.GAME, gameObject.AddComponent<GameState>());

        FSM.RegistEvent(_STATE.MAIN, _ACTION.GO_START, _STATE.START);
        FSM.RegistEvent(_STATE.GAME, _ACTION.GO_START, _STATE.START);

        FSM.RegistEvent(_STATE.START, _ACTION.GO_MAIN, _STATE.MAIN);

        FSM.RegistEvent(_STATE.MAIN, _ACTION.GO_GAME, _STATE.GAME);

        FSM.RegistEvent(_STATE.GAME, _ACTION.GO_MAIN, _STATE.MAIN);

        FSM.Enable(_STATE.START);
    }

    public _STATE CurState()
    {
        return FSM._Current_State;
    }

    public SceneBaseState CurrentBaseState()
    {
        return (SceneBaseState)FSM.Current();
    }

    public T GetState<T>() where T : SceneBaseState
    {
        return gameObject.GetComponent<T>();
    }

    public void ActionEvent(_ACTION NewAction)
    {
        FSM.ChangeState(NewAction);
    }
}
