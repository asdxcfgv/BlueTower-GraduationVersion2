using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyBehaviourTree
{
    public class Enemy_Attack : ActionNode
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
            enemy.GetComponent<EnemyWeaponAI>().FireWeapon();
            return State.Running;
        }
    }
}

