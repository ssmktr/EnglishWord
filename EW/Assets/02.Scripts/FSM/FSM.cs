using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseEvent
{
    int eventID { get; set; }
}

namespace FSM
{
    public class BaseState<PARENT> : MonoBehaviour
    {
        public PARENT _Parent { get; private set; }

        public void Awake()
        {
            enabled = false;
        }

        public virtual void OnInitialize(PARENT _parent)
        {
            _Parent = _parent;
        }

        public virtual void OnEnter(System.Action callback)
        {
            enabled = true;

            if (callback != null)
                callback();
        }

        public virtual void OnExit(System.Action callback)
        {
            enabled = false;

            if (callback != null)
                callback();
        }

        public virtual void OnRelease()
        {
            enabled = false;
            Destroy(this);
        }

        public virtual void OnEvent(IBaseEvent evt) { }
    }

    public class FSM<EVENT, STATE, PARENT> : IEventHandler
    {
        PARENT _Parent;
        public STATE _Current_State { get; private set; }

        protected bool isEntering;

        Dictionary<STATE, Dictionary<EVENT, STATE>> TransitionMap;
        Dictionary<STATE, BaseState<PARENT>> StateMap;

        public BaseState<PARENT> Current()
        {
            return StateMap[_Current_State];
        }

        public FSM(PARENT parent)
        {
            _Parent = parent;
            Initialize();
        }

        void Initialize()
        {
            TransitionMap = new Dictionary<STATE, Dictionary<EVENT, STATE>>();
            StateMap = new Dictionary<STATE, BaseState<PARENT>>();
        }

        public void Release()
        {
            StateMap[_Current_State].OnExit(ReleaseCb);
        }

        void ReleaseCb()
        {
            foreach (BaseState<PARENT> ibs in StateMap.Values)
            {
                ibs.OnRelease();
            }
            StateMap.Clear();
            StateMap = null;
            TransitionMap.Clear();
            TransitionMap = null;
        }

        public bool EnableFlag = false;
        public bool IsEnable { get { return EnableFlag; } }
        public void Enable(STATE state)
        {
            if (EnableFlag)
            {
                ChangeState(state);
            }
            else
            {
                _Current_State = state;
                Enable(true);
            }
        }

        public void Enable(bool flag)
        {
            if (EnableFlag == flag)
                return;
            else
                EnableFlag = flag;

            if (EnableFlag)
            {
                if (StateMap.ContainsKey(_Current_State))
                    StateMap[_Current_State].OnEnter(null);
            }
            else
                StateMap[_Current_State].OnExit(null);
        }

        public void AddState(STATE state, BaseState<PARENT> stateinterface)
        {
            if (!StateMap.ContainsKey(state))
            {
                StateMap.Add(state, stateinterface);
                stateinterface.OnInitialize(_Parent);
            }
        }

        public EVENT GetEvent(STATE baseState, STATE targetState)
        {
            if (TransitionMap.ContainsKey(baseState))
                return default(EVENT);

            foreach (EVENT e in TransitionMap[baseState].Keys)
            {
                if (TransitionMap[baseState][e].Equals(targetState))
                    return e;
            }

            return default(EVENT);
        }

        public void RegistEvent(STATE state, EVENT evt, STATE targetstate)
        {
            try
            {
                if (!TransitionMap.ContainsKey(state))
                    TransitionMap.Add(state, new Dictionary<EVENT, STATE>());
                TransitionMap[state].Add(evt, targetstate);
            }
            catch (System.Exception e)
            {
                Debug.LogError(string.Format("FiniteStateMap Add Error : {0}\n Exception : {1}", evt, e));
                Debug.Break();
            }
        }

        public bool ChangeState(EVENT evt)
        {
            if (isEntering)
            {
                Debug.LogWarning(string.Format("Current : {0}, Target : {1} State Map Error", _Current_State, evt));
                Debug.LogWarning("Current state is already entering");
                return false;
            }

            if (TransitionMap[_Current_State].ContainsKey(evt))
            {
                if (StateMap[_Current_State].Equals(TransitionMap[_Current_State][evt]))
                    return false;

                isEntering = true;
                StateMap[_Current_State].OnExit(delegate ()
                {
                    _Current_State = TransitionMap[_Current_State][evt];
                    StateMap[_Current_State].OnEnter(delegate ()
                    {
                        isEntering = false;
                    });
                });
            }
            else

            Debug.LogWarning(string.Format("Current : {0}, Target : {1} State Map Error", _Current_State, evt));
            return false;
        }

        public bool ChangeState(STATE state)
        {
            if (isEntering)
            {
                Debug.LogWarning(string.Format("Current : {0}", _Current_State));
                Debug.LogWarning("current state is already entering");
                return false;
            }

            if (_Current_State.Equals(state))
                return false;

            if (!StateMap.ContainsKey(state))
                return false;

            isEntering = true;
            StateMap[_Current_State].OnExit(delegate () 
            {
                _Current_State = state;
                StateMap[_Current_State].OnEnter(delegate () 
                {
                    isEntering = false;
                });
            });

            return true;
        }

        public bool CheckEvent(EVENT evt)
        {
            if (TransitionMap.ContainsKey(_Current_State) && TransitionMap[_Current_State].ContainsKey(evt))
            {
                if (StateMap[_Current_State].Equals(TransitionMap[_Current_State][evt]))
                    return false;
                else
                    return true;
            }

            return false;
        }

        public bool HasState(STATE state)
        {
            return StateMap.ContainsKey(state);
        }

        public bool IsEnabled { get; set; }
        public void OnEvent(IBaseEvent evt)
        {
            if (StateMap.ContainsKey(_Current_State))
                StateMap[_Current_State].OnEvent(evt);
        }
    }
}