using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyBehaviourTree
{
    public class Enemy_MoveToPosition : ActionNode
    {
        private Enemy enemy;
        protected override void OnStart()
        {
            enemy = gameObject.GetComponent<Enemy>();
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

