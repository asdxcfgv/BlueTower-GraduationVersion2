using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public  EasyEvent<ReloadWeaponEventArgs> OnReloadWeapon = new EasyEvent<ReloadWeaponEventArgs>();
    
}

public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;

    public ReloadWeaponEventArgs()
    {
        
    }

    public ReloadWeaponEventArgs(Weapon weapon)
    {
        this.weapon = weapon;
    }
}
