using System;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public EasyEvent <DestroyedEvent,DestroyedEventArgs> OnDestroyed = new EasyEvent<DestroyedEvent,DestroyedEventArgs>();
    
}

public class DestroyedEventArgs : EventArgs
{
    public bool playerDied;

    public DestroyedEventArgs()
    {
        
    }

    public DestroyedEventArgs(bool playerDied)
    {
        this.playerDied = playerDied;
    }
}
