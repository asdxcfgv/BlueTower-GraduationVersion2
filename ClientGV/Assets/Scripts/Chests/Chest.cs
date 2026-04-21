using System.Collections;
using QFramework;
using UnityEngine;
using TMPro;
using static GlobalEnums;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]

public class Chest : MonoBehaviour, IUseable
{
   #region Tooltip
    [Tooltip("Set this to the colour to be used for the materialization effect")]
    #endregion Tooltip
    [ColorUsage(false, true)]
    [SerializeField] private Color materializeColor;
    #region Tooltip
    [Tooltip("Set this to the time is will take to materialize the chest")]
    #endregion Tooltip
    [SerializeField] private float materializeTime = 3f;
    #region Tooltip
    [Tooltip("Populate withItemSpawnPoint transform")]
    #endregion Tooltip
    [SerializeField] private Transform itemSpawnPoint;

    [SerializeField] private GameObject tipText;
    
    private int healthPercent;
    private int tempHealthPercent;
    private WeaponDetailsSO weaponDetails;
    private WeaponDetailsSO tempWeaponDetails;
    private int ammoPercent;
    private int tempAmmoPercent;
    private BulletType bulletType;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MaterializeEffect materializeEffect;
    private bool isEnabled = false;
    private ChestState chestState = ChestState.closed;
    private GameObject chestItemGameObject;
    private ChestItem chestItem;
    private TextMeshPro messageTextTMP;

    private void Awake()
    {
        //  Cache components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponentInChildren<TextMeshPro>();
    }

    /// <summary>
    /// Initialize Chest and either make it visible immediately or materialize it
    /// </summary>
    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponDetailsSO weaponDetails, int ammoPercent,BulletType bulletType)
    {
        this.healthPercent = healthPercent;
        this.weaponDetails = weaponDetails;
        this.ammoPercent = ammoPercent;
        this.bulletType = bulletType;
        
        if (shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    /// <summary>
    /// Materialise the chest
    /// </summary>
    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor, materializeTime, spriteRendererArray, GameResources.Instance.litMaterial));

        EnableChest();
    }

    /// <summary>
    /// Enable the chest
    /// </summary>
    private void EnableChest()
    {
        // Set use to enabled
        isEnabled = true;
    }

    /// <summary>
    /// Use the chest - action will vary depending on the chest state
    /// </summary>
    public void UseItem()
    {
        if (!isEnabled) return;

        switch (chestState)
        {
            case ChestState.closed:
                OpenChest();
                break;

            case ChestState.empty:
                return;

            default:
                return;
        }
    }

    void Update()
    {
        if(!isEnabled)
            return;
        ShowTip();


    }

    private void ShowTip()
    {
        
        tipText.Hide();
        
        if(chestState!=ChestState.closed)
            return;
        
        float checkPlayerRadius = 1.5f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(this.transform.position, checkPlayerRadius);
        
        
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.gameObject.CompareTag("Player"))
            {
                tipText.Show();
                return;
            }
        }
    }

    /// <summary>
    /// Open the chest on first use
    /// </summary>
    private void OpenChest()
    {
        animator.SetBool(Global.use, true);

        // chest open sound effect
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        UpdateChestState();
    }

    /// <summary>
    /// Create items based on what should be spawned and the chest state
    /// </summary>
    private void UpdateChestState()
    {
        if (healthPercent != 0)
        {
            chestState = ChestState.healthItem;
            InstantiateHealthItem();
        }
        else if (ammoPercent != 0)
        {
            chestState = ChestState.ammoItem;
            InstantiateAmmoItem();
        }
        else if (weaponDetails != null)
        {
            chestState = ChestState.weaponItem;
            InstantiateWeaponItem();
        }
        else
        {
            chestState = ChestState.empty;
        }
    }

    /// <summary>
    /// Instantiate a chest item
    /// </summary>
    private void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }

    /// <summary>
    /// Instantiate a health item for the player to collect
    /// </summary>
    private void InstantiateHealthItem()
    {
        InstantiateItem();

        chestItem.InitializeWithAnimator(GameResources.Instance.heartIcon,GameResources.Instance.heartAnimator, itemSpawnPoint.position,CollectHealthItem);

        tempHealthPercent = healthPercent;
        
        healthPercent = 0;
        
        chestItem = null;
        
        UpdateChestState();
    }


    /// <summary>
    /// Collect the health item and add it to the players health
    /// </summary>
    private void CollectHealthItem()
    {
        // Add health to player
        GameManager.Instance.GetPlayer().health.AddHealth(tempHealthPercent);

        // Play pickup sound effect
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

    }

    /// <summary>
    /// Instantiate a ammo item for the player to collect
    /// </summary>
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

    /// <summary>
    /// Instantiate a weapon item for the player to collect
    /// </summary>
    private void InstantiateWeaponItem()
    {
        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite, itemSpawnPoint.position,CollectWeaponItem);

        tempWeaponDetails = weaponDetails;
        
        weaponDetails = null;
        
        UpdateChestState();
    }

    /// <summary>
    /// Collect the weapon and add it to the players weapons list
    /// </summary>
    private void CollectWeaponItem()
    {
        
        
        // If the player doesn't already have the weapon, then add to player
        if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(tempWeaponDetails))
        {
            // Add weapon to player
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(tempWeaponDetails);
        }
        else
        {
            // Update ammo for current weapon
            GameManager.Instance.GetPlayer().playerResources.AddAmmo(tempWeaponDetails.weaponClipAmmoCapacity,tempWeaponDetails.usingBulletType);
        }
        
        // Play pickup sound effect
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
    }
    
}
