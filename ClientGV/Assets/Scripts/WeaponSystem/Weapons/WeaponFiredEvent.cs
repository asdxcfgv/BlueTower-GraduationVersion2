using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponFiredEvent : MonoBehaviour
{
    public EasyEvent<WeaponFiredEventArgs> OnWeaponFired = new EasyEvent<WeaponFiredEventArgs>();
    
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon weapon;

    public WeaponFiredEventArgs()
    {
        
    }

    public WeaponFiredEventArgs(Weapon weapon)
    {
        this.weapon = weapon;
    }
}
