using System.Collections;
using System.Collections.Generic;
using System;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponReloadedEvent : MonoBehaviour
{
    public EasyEvent<WeaponReloadedEventArgs> OnWeaponReloaded = new EasyEvent<WeaponReloadedEventArgs>();
    
}

public class WeaponReloadedEventArgs : EventArgs
{
    public Weapon weapon;

    public WeaponReloadedEventArgs()
    {
        
    }

    public WeaponReloadedEventArgs(Weapon weapon)
    {
        this.weapon = weapon;
    }
}
