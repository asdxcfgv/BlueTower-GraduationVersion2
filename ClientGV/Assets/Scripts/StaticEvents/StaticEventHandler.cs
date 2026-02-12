using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;


public static class StaticEventHandler
{
    public static  EasyEvent<RoomChangedEventArgs> OnRoomChanged = new EasyEvent<RoomChangedEventArgs>();
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;

    public RoomChangedEventArgs(Room room)
    {
        this.room = room;
    }
}
