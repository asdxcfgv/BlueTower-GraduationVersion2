using System;
using QFramework;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthEvent : MonoBehaviour
{
    public EasyEvent <HealthEventArgs> OnHealthChanged = new EasyEvent<HealthEventArgs>();
}

public class HealthEventArgs : EventArgs
{
    public float healthPercent;
    public int healthAmount;
    public int damageAmount;

    public HealthEventArgs(float healthPercent, int healthAmount, int damageAmount)
    {
        this.healthPercent = healthPercent;
        this.healthAmount = healthAmount;
        this.damageAmount = damageAmount;
    }

    public HealthEventArgs()
    {
        
    }
}
