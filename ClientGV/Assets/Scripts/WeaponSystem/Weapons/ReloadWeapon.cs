using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]

[DisallowMultipleComponent]
public class ReloadWeapon : MonoBehaviour
{
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponReloadedEvent weaponReloadedEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private ActiveWeapon activeWeapon;
    private Coroutine reloadWeaponCoroutine;

    private void Awake()
    {
        // Load components
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void OnEnable()
    {
        // subscribe to reload weapon event
        reloadWeaponEvent.OnReloadWeapon.Register(ReloadWeaponEvent_OnReloadWeapon);

        // Subscribe to set active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon.Register(SetActiveWeaponEvent_OnSetActiveWeapon);
    }

    private void OnDisable()
    {
        // unsubscribe from reload weapon event
        reloadWeaponEvent.OnReloadWeapon.UnRegister(ReloadWeaponEvent_OnReloadWeapon);

        // Unsubscribe from set active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon.UnRegister(SetActiveWeaponEvent_OnSetActiveWeapon);
    }

    /// <summary>
    /// Handle reload weapon event
    /// </summary>
    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEventArgs reloadWeaponEventArgs)
    {
        StartReloadWeapon(reloadWeaponEventArgs);
    }

    /// <summary>
    /// Start reloading the weapon
    /// </summary>
    private void StartReloadWeapon(ReloadWeaponEventArgs reloadWeaponEventArgs)
    {
        if (reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }

        float x = Global.GetAnimationClipLength(activeWeapon.GetAnimator(), "Gun_LoadIn");
        
        activeWeapon.GetAnimator().speed = Global.GetAnimationClipLength(activeWeapon.GetAnimator(),"Gun_LoadIn")/reloadWeaponEventArgs.weapon.weaponDetails.weaponReloadTime;
        
        activeWeapon.GetAnimator().Play("Gun_LoadIn");
        
        
        reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(reloadWeaponEventArgs.weapon, reloadWeaponEventArgs.topUpAmmoPercent));
    }

    /// <summary>
    /// Reload weapon coroutine
    /// </summary>
    private IEnumerator ReloadWeaponRoutine(Weapon weapon, int topUpAmmoPercent)
    {
        // Set weapon as reloading
        weapon.isWeaponReloading = true;

        // Update reload progress timer
        while (weapon.weaponReloadTimer < weapon.weaponDetails.weaponReloadTime)
        {
            weapon.weaponReloadTimer += Time.deltaTime;
            yield return null;
        }

        // If total ammo is to be increased then update
        if (topUpAmmoPercent != 0)
        {
            int ammoIncrease = Mathf.RoundToInt((weapon.weaponDetails.weaponAmmoCapacity * topUpAmmoPercent) / 100f);

            int totalAmmo = weapon.weaponRemainingAmmo + ammoIncrease;

            if (totalAmmo > weapon.weaponDetails.weaponAmmoCapacity)
            {
                weapon.weaponRemainingAmmo = weapon.weaponDetails.weaponAmmoCapacity;
            }
            else
            {
                weapon.weaponRemainingAmmo = totalAmmo;
            }
        }

        // If weapon has infinite ammo then just refil the clip
        if (weapon.weaponDetails.hasInfiniteAmmo)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponDetails.weaponClipAmmoCapacity;
        }
        // else if not infinite ammo then if remaining ammo is greater than the amount required to
        // refill the clip, then fully refill the clip
        else if (weapon.weaponRemainingAmmo >= weapon.weaponDetails.weaponClipAmmoCapacity)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponDetails.weaponClipAmmoCapacity;
        }
        // else set the clip to the remaining ammo
        else
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponRemainingAmmo;
        }

        // Reset weapon reload timer
        weapon.weaponReloadTimer = 0f;

        // Set weapon as not reloading
        weapon.isWeaponReloading = false;
        
        activeWeapon.GetAnimator().Rebind();

        // Call weapon reloaded event
        weaponReloadedEvent.OnWeaponReloaded.Trigger(new WeaponReloadedEventArgs(weapon));

    }

    /// <summary>
    /// Set active weapon event handler
    /// </summary>
    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {
        if (setActiveWeaponEventArgs.weapon.isWeaponReloading)
        {
            if (reloadWeaponCoroutine != null)
            {
                StopCoroutine(reloadWeaponCoroutine);
            }

            reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(setActiveWeaponEventArgs.weapon, 0));
        }
    }


}
