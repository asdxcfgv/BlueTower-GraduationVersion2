using QFramework;
using UnityEngine;
using static GlobalEnums;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy enemy;
    
    private DestroyedEvent destroyedEvent;

    private bool isDead;
    
    private void Awake()
    {
        // Load components
        enemy = GetComponent<Enemy>();
        
        destroyedEvent = GetComponent<DestroyedEvent>();
    }

    private void OnEnable()
    {
        // Subscribe to movement event
        enemy.movementToPositionEvent.OnMovementToPosition.Register(MovementToPositionEvent_OnMovementToPosition);

        enemy.fireWeaponEvent.OnFireWeapon.Register(FireWeaponEvent_OnFire);
        
        destroyedEvent.OnDestroyed.Register(DestroyedEvent_OnDestroyed);
        
        // Subscribe to idle event
        enemy.idleEvent.OnIdle.Register(IdleEvent_OnIdle);

        // Subscribe to weapon aim event
        enemy.aimWeaponEvent.OnWeaponAim.Register(AimWeaponEvent_OnWeaponAim);

        isDead = false;
    }

    private void OnDisable()
    {
        // Unsubscribe from movement event
        enemy.movementToPositionEvent.OnMovementToPosition.UnRegister(MovementToPositionEvent_OnMovementToPosition);
        
        enemy.fireWeaponEvent.OnFireWeapon.UnRegister(FireWeaponEvent_OnFire);
        
        destroyedEvent.OnDestroyed.Register(DestroyedEvent_OnDestroyed);
        
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

    private void FireWeaponEvent_OnFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        SetAttackAnimationParameters();
    }

    private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent,DestroyedEventArgs destroyedEventArgs)
    {
        SetDeadAnimationParameters();
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
        if (!isDead)
        {
            enemy.animator.SetBool(Global.isIdle, false);
            enemy.animator.SetBool(Global.isMoving, true);
            enemy.animator.SetBool(Global.isAttacking, false);
        }
        
    }


    /// <summary>
    /// Set idle animation parameters
    /// </summary>
    private void SetIdleAnimationParameters()
    {
        if (!isDead)
        {
            // Set idle
            enemy.animator.SetBool(Global.isMoving, false);
            enemy.animator.SetBool(Global.isAttacking, false);
            enemy.animator.SetBool(Global.isIdle, true);
        }
        
    }

    private void SetAttackAnimationParameters()
    {
        if (!isDead)
        {
            enemy.animator.SetBool(Global.isAttacking, true);
            enemy.animator.SetBool(Global.isIdle,false);
            enemy.animator.SetBool(Global.isMoving,false);
        }
        
    }

    private void SetDeadAnimationParameters()
    {
        enemy.animator.SetBool(Global.isAttacking, false);
        enemy.animator.SetBool(Global.isIdle,false);
        enemy.animator.SetBool(Global.isMoving,false);
        
        enemy.animator.SetTrigger(Global.isDead);
        
        isDead = true;

        ActionKit.Delay(GetDeadAnimationClipLength()/enemy.animator.speed, () =>
        {
            Destroy(gameObject);
        }).Start(this);
    }

    private float GetDeadAnimationClipLength()
    {
        if (enemy.animator.GetBool(Global.aimUp))
        {
            return Global.GetAnimationClipLengthBySuffix(enemy.animator, "Back_Destroy");
        }
        else if (enemy.animator.GetBool(Global.aimDown))
        {
            return Global.GetAnimationClipLengthBySuffix(enemy.animator, "Front_Destroy");
        }
        else if (enemy.animator.GetBool(Global.aimLeft))
        {
            return Global.GetAnimationClipLengthBySuffix(enemy.animator, "Left_Destroy");
        }
        else
        {
            return Global.GetAnimationClipLengthBySuffix(enemy.animator, "Right_Destroy");
        }
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
