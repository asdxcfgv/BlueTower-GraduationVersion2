using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(HitShake))]
public class DestroyableItem : MonoBehaviour
{
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
    //[SerializeField] private SoundEffectSO destroySoundEffect;
    private Animator animator;
    private HitShake hitShake;
    private BoxCollider2D boxCollider2D;
    private HealthEvent healthEvent;
    private Health health;
    private ReceiveContactDamage receiveContactDamage;
    
    private bool isDestroyed;

    private void Awake()
    {
        isDestroyed = false;
        
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        health.SetStartingHealth(startingHealthAmount);
        receiveContactDamage = GetComponent<ReceiveContactDamage>();
        hitShake = GetComponent<HitShake>();
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
        if (healthEventArgs.damageAmount > 0&&!isDestroyed)
        {
            hitShake.PlayShake();
        }
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
        Destroy(this);

    }
}
