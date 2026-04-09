using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyBehaviourTree
{
    public class MoveToPosition : ActionNode
    {
        protected override void OnStart() 
        {
            
        }

        protected override void OnStop() 
        {
        }

        protected override State OnUpdate() 
        {
            return State.Running;
        }
    }
}

