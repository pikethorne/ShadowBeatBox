using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class SimpleAI : MonoBehaviour
{
    public bool switchState = false;

    public StateController<SimpleAI> stateController { get; set; }

    private void Start()
    {
        stateController = new StateController<SimpleAI>(this);
        stateController.ChangeState(IdleState.Instance);
    }

    //private void Update()
    //{
    //    if (Global.switchStateFlipper == true)
    //    {
    //        StateSwitch();
    //        stateController.Update();
    //        Global.switchStateFlipper = false;
    //    }
    //}

    //void StateSwitch()
    //{
    //    switchState = !switchState;
    //}

}

//public class Global
//{
//    public static bool switchStateFlipper = false;
//}