using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using QFramework;
using static GlobalEnums;

[DisallowMultipleComponent]
public class FireWeaponEvent : MonoBehaviour
{
    public EasyEvent<FireWeaponEventArgs> OnFireWeapon = new EasyEvent<FireWeaponEventArgs>();
}

public class FireWeaponEventArgs : EventArgs
{
    public bool fire;
    public bool firePreviousFrame;
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;

    public FireWeaponEventArgs()
    {
        
    }

    public FireWeaponEventArgs(bool fire, bool firePreviousFrame, AimDirection aimDirection, float aimAngle,
        float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        this.fire = fire;
        this.firePreviousFrame = firePreviousFrame;
        this.aimDirection = aimDirection;
        this.aimAngle = aimAngle;
        this.weaponAimAngle = weaponAimAngle;
        this.weaponAimDirectionVector = weaponAimDirectionVector;
    }
}
