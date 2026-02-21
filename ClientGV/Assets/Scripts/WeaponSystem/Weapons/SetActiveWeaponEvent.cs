using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class SetActiveWeaponEvent : MonoBehaviour
{
    public  EasyEvent<SetActiveWeaponEventArgs> OnSetActiveWeapon = new EasyEvent<SetActiveWeaponEventArgs>();
}

public class SetActiveWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    
    public SetActiveWeaponEventArgs()
    {
        
    }
    
    public SetActiveWeaponEventArgs(Weapon weapon)
    {
        this.weapon = weapon;
    }
}