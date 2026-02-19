using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        // Subscribe to movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity.Register(MovementByVelocityEvent_OnMovementByVelocity);
        

        // Subscribe to idle event
        player.idleEvent.OnIdle.Register(IdleEvent_OnIdle);

        // Subscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim.Register(AimWeaponEvent_OnWeaponAim);
    }

    private void OnDisable()
    {
        // Unsubscribe from movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity.UnRegister(MovementByVelocityEvent_OnMovementByVelocity);
        

        // Unsubscribe from idle event
        player.idleEvent.OnIdle.UnRegister(IdleEvent_OnIdle);

        // Unsubscribe from weapon aim event event
        player.aimWeaponEvent.OnWeaponAim.UnRegister(AimWeaponEvent_OnWeaponAim);
    }

    /// <summary>
    /// On movement by velocity event handler
    /// </summary>
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityArgs movementByVelocityArgs)
    {
        InitializeRollAnimationParameters();
        SetMovementAnimationParameters();
    }
    

    /// <summary>
    /// On idle event handler
    /// </summary>
    private void IdleEvent_OnIdle()
    {
        InitializeRollAnimationParameters();
        SetIdleAnimationParameters();
    }

    /// <summary>
    /// On weapon aim event handler
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();
        InitializeRollAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// Initialise aim animation parameters
    /// </summary>
    private void InitializeAimAnimationParameters()
    {
        player.animator.SetBool(Global.aimUp, false);
        player.animator.SetBool(Global.aimRight, false);
        player.animator.SetBool(Global.aimLeft, false);
        player.animator.SetBool(Global.aimDown, false);
    }

    private void InitializeRollAnimationParameters()
    {
        
    }


    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(Global.isMoving, true);
        player.animator.SetBool(Global.isIdle, false);
    }
    

    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetIdleAnimationParameters()
    {
        player.animator.SetBool(Global.isMoving, false);
        player.animator.SetBool(Global.isIdle, true);
    }

    /// <summary>
    /// Set aim animation parameters
    /// </summary>
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        // Set aim direction
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Global.aimUp, true);
                break;

            case AimDirection.Right:
                player.animator.SetBool(Global.aimRight, true);
                break;

            case AimDirection.Left:
                player.animator.SetBool(Global.aimLeft, true);
                break;

            case AimDirection.Down:
                player.animator.SetBool(Global.aimDown, true);
                break;

        }

    }

}
