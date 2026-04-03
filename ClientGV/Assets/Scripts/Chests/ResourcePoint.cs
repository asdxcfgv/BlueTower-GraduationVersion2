using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GlobalEnums;

public class ResourcePoint : MonoBehaviour
{
    [ColorUsage(false, true)]
    [SerializeField] private Color materializeColor;
    
    #region Header HEALTH
    [Header("HEALTH")]
    #endregion Header HEALTH
    #region Tooltip
    [Tooltip("What the starting health for this destroyable item should be")]
    #endregion Tooltip
    [SerializeField] private int startingHealthAmount = 1;
    [SerializeField] private bool isObstacleWhenDestroyed;
    #region SOUND EFFECT

    [Header("SOUND EFFECT")]

    #endregion SOUND EFFECT
    #region Tooltip

    [Tooltip("The sound effect when this item is destroyed")]

    #endregion Tooltip
    
    private int healthPercent;
    private int tempHealthPercent;
    private int ammoPercent;
    private int tempAmmoPercent;
    private BulletType bulletType;
    private bool isDestroyed;
    
    
    [SerializeField] private Transform itemSpawnPoint;

    private ResourcePointState resourcePointState;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private HealthEvent healthEvent;
    private Health health;
    private ReceiveContactDamage receiveContactDamage;
    private GameObject chestItemGameObject;
    private ChestItem chestItem;
    private TextMeshPro messageTextTMP;

    public void Initialize(int healthPercent, int ammoPercent,BulletType bulletType)
    {
        this.bulletType = bulletType;
        this.healthPercent = healthPercent;
        this.ammoPercent = ammoPercent;

        isDestroyed = false;
        resourcePointState = ResourcePointState.notDestroyed;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        health.SetStartingHealth(startingHealthAmount);
        receiveContactDamage = GetComponent<ReceiveContactDamage>();
    }
    
    private void OnEnable()
    {
        healthEvent.OnHealthChanged.Register(HealthEvent_OnHealthLost);
    }


    private void OnDisable()
    {
        healthEvent.OnHealthChanged.UnRegister(HealthEvent_OnHealthLost);
    }
    
    private void HealthEvent_OnHealthLost(HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0f&&!isDestroyed)
        {
            isDestroyed = true;
            StartCoroutine(PlayAnimation());
        }
    }
    
    private IEnumerator PlayAnimation()
    {
        // Destroy the trigger collider
        if (!isObstacleWhenDestroyed)
        {
            Destroy(boxCollider2D);
        }
        

        // Play sound effect
        /*if (destroySoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(destroySoundEffect);
        }*/

        // Trigger the destroy animation
        animator.SetBool(Global.destroy, true);


        // Let the animation play through
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(Global.stateDestroyed))
        {
            yield return null;
        }

        // Then destroy all components other than the Sprite Renderer to just display the final
        // sprite in the animation
        Destroy(animator);
        Destroy(receiveContactDamage);
        Destroy(health);
        Destroy(healthEvent);

        UpdateChestState();

    }
    
    private void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }
    
    /// <summary>
    /// Create items based on what should be spawned and the chest state
    /// </summary>
    private void UpdateChestState()
    {
        if (healthPercent != 0)
        {
            resourcePointState = ResourcePointState.healthItem;
            InstantiateHealthItem();
        }
        else if (ammoPercent != 0)
        {
            resourcePointState = ResourcePointState.ammoItem;
            InstantiateAmmoItem();
        }
        else
        {
            resourcePointState = ResourcePointState.obstacle;
        }
    }
    
    private void InstantiateHealthItem()
    {
        InstantiateItem();

        chestItem.InitializeWithAnimator(GameResources.Instance.heartIcon,GameResources.Instance.heartAnimator, itemSpawnPoint.position,CollectHealthItem);

        tempHealthPercent = healthPercent;
        
        healthPercent = 0;
        
        chestItem = null;
        
        UpdateChestState();
    }
    
    private void CollectHealthItem()
    {
        // Add health to player
        GameManager.Instance.GetPlayer().health.AddHealth(tempHealthPercent);

        // Play pickup sound effect
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);
        
    }
    
    private void InstantiateAmmoItem()
    {
        InstantiateItem();

        switch (bulletType)
        {
            case BulletType.normal:
                chestItem.InitializeWithAnimator(GameResources.Instance.normalBulletIcon, GameResources.Instance.normalBulletAnimator, itemSpawnPoint.position,CollectAmmoItem);
                break;
            case BulletType.electron:
                chestItem.InitializeWithAnimator(GameResources.Instance.electronBulletIcon, GameResources.Instance.electronBulletAnimator, itemSpawnPoint.position,CollectAmmoItem);
                break;
            case BulletType.boom:
                chestItem.InitializeWithAnimator(GameResources.Instance.boomBulletIcon, GameResources.Instance.boomBulletAnimator, itemSpawnPoint.position,CollectAmmoItem);
                break;
            default:
                break;
        }

        tempAmmoPercent = ammoPercent;
        
        ammoPercent = 0;
        
        UpdateChestState();

        chestItem = null;
    }
    
    /// <summary>
    /// Collect an ammo item and add it to the ammo in the players current weapon
    /// </summary>
    private void CollectAmmoItem()
    {
        // Update ammo for current weapon
        GameManager.Instance.GetPlayer().playerResources.AddAmmo(tempAmmoPercent,bulletType);

        // Play pickup sound effect
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);
        
    }
}
