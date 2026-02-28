using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;


public static class StaticEventHandler
{
    public static  EasyEvent<RoomChangedEventArgs> OnRoomChanged = new EasyEvent<RoomChangedEventArgs>();
    
    // Room enemies defeated event
    public static EasyEvent<RoomEnemiesDefeatedArgs> OnRoomEnemiesDefeated = new EasyEvent<RoomEnemiesDefeatedArgs>();
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;

    public RoomChangedEventArgs(Room room)
    {
        this.room = room;
    }

    public RoomChangedEventArgs()
    {
        
    }
}

public class RoomEnemiesDefeatedArgs : EventArgs
{
    public Room room;
    
    public RoomEnemiesDefeatedArgs(Room room)
    {
        this.room = room;
    }

    public RoomEnemiesDefeatedArgs()
    {
        
    }
}
