using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using static GlobalEnums;

[DisallowMultipleComponent]
public class AimWeaponEvent : MonoBehaviour
{
    public  EasyEvent<AimWeaponEventArgs> OnWeaponAim = new EasyEvent<AimWeaponEventArgs>();
    
}

public class AimWeaponEventArgs : EventArgs
{
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;

    public AimWeaponEventArgs()
    {
        
    }
    
    public AimWeaponEventArgs(AimDirection aimDirection,float aimAngle,float weaponAimAngle,Vector3 weaponAimDirectionVector)
    {
        this.aimDirection = aimDirection;
        this.aimAngle = aimAngle;
        this.weaponAimAngle = weaponAimAngle;
        this.weaponAimDirectionVector = weaponAimDirectionVector;
    }
}
