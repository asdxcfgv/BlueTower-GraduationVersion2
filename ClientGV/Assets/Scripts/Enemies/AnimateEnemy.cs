using UnityEngine;
using static GlobalEnums;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        // Load components
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        // Subscribe to movement event
        enemy.movementToPositionEvent.OnMovementToPosition.Register(MovementToPositionEvent_OnMovementToPosition);

        // Subscribe to idle event
        enemy.idleEvent.OnIdle.Register(IdleEvent_OnIdle);

        // Subscribe to weapon aim event
        enemy.aimWeaponEvent.OnWeaponAim.Register(AimWeaponEvent_OnWeaponAim);
    }

    private void OnDisable()
    {
        // Unsubscribe from movement event
        enemy.movementToPositionEvent.OnMovementToPosition.UnRegister(MovementToPositionEvent_OnMovementToPosition);

        // Unsubscribe from idle event
        enemy.idleEvent.OnIdle.UnRegister(IdleEvent_OnIdle);

        // Unsubscribe from weapon aim event event
        enemy.aimWeaponEvent.OnWeaponAim.UnRegister(AimWeaponEvent_OnWeaponAim);
    }

    /// <summary>
    /// On weapon aim event handler
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitialiseAimAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// On movement event handler
    /// </summary>
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionArgs movementToPositionArgs)
    {
        SetMovementAnimationParameters();
    }

    /// <summary>
    /// On idle event handler
    /// </summary>
    private void IdleEvent_OnIdle()
    {
        SetIdleAnimationParameters();
    }

    /// <summary>
    /// Initialise aim animation parameters
    /// </summary>
    private void InitialiseAimAnimationParameters()
    {
        enemy.animator.SetBool(Global.aimUp, false);
        enemy.animator.SetBool(Global.aimRight, false);
        enemy.animator.SetBool(Global.aimLeft, false);
        enemy.animator.SetBool(Global.aimDown, false);
    }

    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        // Set Moving
        enemy.animator.SetBool(Global.isIdle, false);
        enemy.animator.SetBool(Global.isMoving, true);
    }


    /// <summary>
    /// Set idle animation parameters
    /// </summary>
    private void SetIdleAnimationParameters()
    {
        // Set idle
        enemy.animator.SetBool(Global.isMoving, false);
        enemy.animator.SetBool(Global.isIdle, true);
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
                enemy.animator.SetBool(Global.aimUp, true);
                break;
            
            

            case AimDirection.Right:
                enemy.animator.SetBool(Global.aimRight, true);
                break;

            case AimDirection.Left:
                enemy.animator.SetBool(Global.aimLeft, true);
                break;

            case AimDirection.Down:
                enemy.animator.SetBool(Global.aimDown, true);
                break;
        }
    }
}
