using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class IdleEvent : MonoBehaviour
{
    public  EasyEvent OnIdle = new EasyEvent();
}
