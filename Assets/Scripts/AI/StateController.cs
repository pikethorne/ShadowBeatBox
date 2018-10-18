using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateStuff
{
    public class StateController<t>
    {
        public State<t> currentState { get; private set; }
        public t owner;

        public StateController(t o)
        {
            owner = o;
            currentState = null;
        }

        public void ChangeState(State<t> _newstate)
        {
            if(currentState != null)
                currentState.ExitState(owner);
            
            currentState = _newstate;
            currentState.EnterState(owner);

        }

        public void Update()
        {
            if (currentState != null)
                currentState.UpdateState(owner);
        }
    }

    public abstract class State<t>
    {
        public abstract void EnterState(t _owner);
        public abstract void ExitState(t _owner);
        public abstract void UpdateState(t _owner);
    }
}