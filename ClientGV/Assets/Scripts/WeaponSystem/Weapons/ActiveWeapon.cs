using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the SpriteRenderer on the child Weapon gameobject")]
    #endregion
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    #region Tooltip
    [Tooltip("Populate with the PolygonCollider2D on the child Weapon gameobject")]
    #endregion
    [SerializeField] private PolygonCollider2D weaponPolygonCollider2D;
    #region Tooltip
    [Tooltip("Populate with the Transform on the WeaponShootPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponShootPositionTransform;
    #region Tooltip
    [Tooltip("Populate with the Transform on the WeaponEffectPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponEffectPositionTransform;

    [SerializeField] private Animator weaponAnimator;

    private SetActiveWeaponEvent setWeaponEvent;
    private Weapon currentWeapon;


    private void Awake()
    {
        // Load components
        setWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        setWeaponEvent.OnSetActiveWeapon.Register(SetWeaponEvent_OnSetActiveWeapon);
    }

    private void OnDisable()
    {
        setWeaponEvent.OnSetActiveWeapon.UnRegister(SetWeaponEvent_OnSetActiveWeapon);
    }

    private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {
        SetWeapon(setActiveWeaponEventArgs.weapon);
    }

    private void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        
        // 先换图片！！！
        if (currentWeapon.weaponDetails.weaponSprite != null)
        {
            
            weaponSpriteRenderer.sprite = currentWeapon.weaponDetails.weaponSprite;
            
        }

        //再换动画机！！！
        if (currentWeapon.weaponDetails.animatorController != null)
        {
            weaponAnimator.runtimeAnimatorController = currentWeapon.weaponDetails.animatorController;
        }
        
        if (currentWeapon.weaponDetails.weaponSprite != null)
        {
            weaponSpriteRenderer.sprite = currentWeapon.weaponDetails.weaponSprite;
            Debug.Log("成功设置Sprite为: " + weaponSpriteRenderer.sprite.name);
        }
        

        // If the weapon has a polygon collider and a sprite then set it to the weapon sprite physics shape
        if (weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
        {
            // Get sprite physics shape - this returns the sprite physics shape points as a list of Vector2s
            List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();
            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList);

            // Set polygon collider on weapon to pick up physics shap for sprite - set collider points to sprite physics shape points
            weaponPolygonCollider2D.points = spritePhysicsShapePointsList.ToArray();

        }

        // Set weapon shoot position
        weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.weaponShootPosition;
        
    }

    public AmmoDetailsSO GetCurrentAmmo()
    {
        return currentWeapon.weaponDetails.weaponCurrentAmmo;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public Vector3 GetShootPosition()
    {
        return weaponShootPositionTransform.position;
    }

    public Vector3 GetShootEffectPosition()
    {
        return weaponEffectPositionTransform.position;
    }

    public void RemoveCurrentWeapon()
    {
        currentWeapon = null;
    }

    public Animator GetAnimator()
    {
        return weaponAnimator;
    }
}
