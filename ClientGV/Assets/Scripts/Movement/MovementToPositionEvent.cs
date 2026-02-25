using System;
using System.Collections;
using System;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public  EasyEvent<MovementToPositionArgs> OnMovementToPosition;
}

public class MovementToPositionArgs : EventArgs
{
    public Vector3 movePosition;
    public Vector3 currentPosition;
    public float moveSpeed;
    public Vector2 moveDirection;
    public bool isRolling;

    public MovementToPositionArgs(Vector3 movePosition,Vector3 currentPosition, float moveSpeed, Vector2 moveDirection, bool isRolling)
    {
        this.movePosition = movePosition;
        this.moveSpeed = moveSpeed;
        this.moveDirection = moveDirection;
        this.isRolling = isRolling;
        this.currentPosition = currentPosition;
    }

    public MovementToPositionArgs()
    {
        
    }
}
