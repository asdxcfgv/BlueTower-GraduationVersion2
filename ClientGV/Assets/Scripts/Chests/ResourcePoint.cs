using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GlobalEnums;

public class ResourcePoint : MonoBehaviour,IUseable
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
    private int ammoPercent;
    
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

    public void Initialize(int healthPercent, int ammoPercent)
    {
        
        
        this.healthPercent = healthPercent;
        this.ammoPercent = ammoPercent;

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
        if (healthEventArgs.healthAmount <= 0f)
        {
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
    public void UseItem()
    {

        switch (resourcePointState)
        {
            case ResourcePointState.notDestroyed:
                return;
            
            case ResourcePointState.healthItem:
                CollectHealthItem();
                break;

            case ResourcePointState.ammoItem:
                CollectAmmoItem();
                break;

            case ResourcePointState.obstacle:
                return;

            default:
                return;
        }
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

        chestItem.InitializeWithAnimator(GameResources.Instance.heartIcon,GameResources.Instance.heartAnimator, healthPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }
    
    private void CollectHealthItem()
    {
        // Check item exists and has been materialized
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        // Add health to player
        GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);

        // Play pickup sound effect
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

        healthPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }
    
    private void InstantiateAmmoItem()
    {
        InstantiateItem();

        chestItem.InitializeWithAnimator(GameResources.Instance.bulletIcon,GameResources.Instance.bulletAnimator, ammoPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }
    
    /// <summary>
    /// Collect an ammo item and add it to the ammo in the players current weapon
    /// </summary>
    private void CollectAmmoItem()
    {
        // Check item exists and has been materialized
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer();

        // Update ammo for current weapon
        player.reloadWeaponEvent.OnReloadWeapon.Trigger(new ReloadWeaponEventArgs(player.activeWeapon.GetCurrentWeapon(), ammoPercent));

        // Play pickup sound effect
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

        ammoPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }
}
