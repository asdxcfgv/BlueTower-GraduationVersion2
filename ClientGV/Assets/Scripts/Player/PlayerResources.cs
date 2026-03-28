using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class PlayerResources : MonoBehaviour
{
    private Player player;
    
    private int normalBullet;
    private int electronBullet;
    private int boomBullet;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();
    }
    
    private void OnEnable()
    {
        // Subscribe to fire weapon event.
        player.weaponReloadedEvent.OnWeaponReloaded.Register(WeaponReloadedEvent_OnWeaponReloaded);
    }

    private void OnDisable()
    {
        // Unsubscribe from fire weapon event.
        player.weaponReloadedEvent.OnWeaponReloaded.UnRegister(WeaponReloadedEvent_OnWeaponReloaded);
    }

    public void InitializeResources(int normal,int electron,int boom)
    {
        normalBullet = normal;
        electronBullet = electron;
        boomBullet = boom;
    }

    public int GetAmmoNum(BulletType type)
    {
        switch (type)
        {
            case BulletType.normal:
                return normalBullet;
            case BulletType.electron:
                return electronBullet;
            case BulletType.boom:
                return boomBullet;
            default:
                return 0;
        }
    }
    
    private void WeaponReloadedEvent_OnWeaponReloaded(WeaponReloadedEventArgs weaponReloadedEventArgs)
    {
        WeaponReloaded(weaponReloadedEventArgs.weapon,weaponReloadedEventArgs.ammoCost);
    }

    private void WeaponReloaded(Weapon weapon, int ammoCost)
    {
        switch (weapon.weaponDetails.usingBulletType)
        {
            case BulletType.normal:
                normalBullet-=ammoCost;
                if (normalBullet < 0)
                {
                    normalBullet = 0;
                }
                break;
            case BulletType.electron:
                electronBullet-=ammoCost;
                if (electronBullet < 0)
                {
                    electronBullet = 0;
                }
                break;
            case BulletType.boom:
                boomBullet-=ammoCost;
                if (boomBullet < 0)
                {
                    boomBullet = 0;
                }
                break;
            default:
                break;
        }
    }
}
