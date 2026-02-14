using System.Collections;
using System.Collections.Generic;
using System;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementByVelocityEvent : MonoBehaviour
{
    public EasyEvent<MovementByVelocityArgs> OnMovementByVelocity = new EasyEvent<MovementByVelocityArgs>();

}

public class MovementByVelocityArgs : EventArgs
{
    public Vector2 moveDirection;
    public float moveSpeed;

    public MovementByVelocityArgs()
    {
        
    }
    
    public MovementByVelocityArgs(Vector2 moveDirection, float moveSpeed)
    {
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
    }
}
