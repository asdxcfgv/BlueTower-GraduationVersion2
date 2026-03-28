using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerResourcesChangedEvent : MonoBehaviour
{
    public  EasyEvent<PlayerResourcesChangedEventArgs> OnPlayerResourcesChanged = new EasyEvent<PlayerResourcesChangedEventArgs>();
}

public class PlayerResourcesChangedEventArgs : EventArgs
{


    public PlayerResourcesChangedEventArgs()
    {
        
    }
    

}
