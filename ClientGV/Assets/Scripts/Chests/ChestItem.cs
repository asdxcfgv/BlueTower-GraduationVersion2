using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestItem : MonoBehaviour
{
    private Action collectAction;
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    [Header("物品设置")]
    [Tooltip("弧线最大高度（带小数点）")]
    [SerializeField]private float maxHeight = 2.5f;
    [Tooltip("弧线运动持续时间（带小数点）")]
    [SerializeField]private float arcDuration = 1.0f;    
    [Tooltip("悬浮振幅（带小数点）")]
    [SerializeField]private float floatAmplitude = 0.1f;
    [Tooltip("悬浮频率（带小数点）")]
    [SerializeField]private float floatFrequency = 2.0f;
    [Tooltip("旋转速度（带小数点）")]
    [SerializeField]private float rotationSpeed = 180.0f;
    [Tooltip("漂浮速度")]
    [SerializeField]private float floatSpeed;
    [Tooltip("弹出力度")]
    [SerializeField]private float popForce;
    
    [Header("自动吸附设置")]
    [Tooltip("吸附距离（带小数点）")]
    [SerializeField]private float attractionDistance = 2.0f;
    [Tooltip("吸附速度（带小数点）")]
    [SerializeField]private float attractionSpeed = 3.0f;
    [Tooltip("磁力效果强度（带小数点）")]
    [SerializeField]private float magnetEffectIntensity = 0.5f;
    
    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isFloating = false;
    private bool isAttracting = false;
    private Transform playerTransform;
    
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // 确保刚体设置正确
        if (rb != null)
        {
            rb.gravityScale = 0.0f;        // 初始关闭重力
            rb.drag = 0.5f;                // 空气阻力（带小数点）
            rb.angularDrag = 0.5f;         // 旋转阻力（带小数点）
        }
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Initialize the chest item
    /// </summary>
    public void InitializeWithAnimator(Sprite sprite,RuntimeAnimatorController animatorController, Vector3 spawnPosition,Action callback)
    {
        spriteRenderer.sprite = sprite;
        transform.position = spawnPosition;
        animator.runtimeAnimatorController = animatorController;
        collectAction = callback;
        
        startPosition = transform.position;
        
        // 计算弧线终点位置（稍微偏移）
        float randomOffsetX = Random.Range(-1.5f, 1.5f);  // 随机X偏移（带小数点）
        endPosition = startPosition + new Vector3(randomOffsetX, 0.0f, 0.0f);

        // 设置悬浮频率
        floatFrequency = floatSpeed;

        // 开始弧线运动
        StartCoroutine(ArcMovement(popForce));
        
    }

    public void Initialize(Sprite sprite, Vector3 spawnPosition,Action callback)
    {
        spriteRenderer.sprite = sprite;
        transform.position = spawnPosition;
        collectAction = callback;

        startPosition = transform.position;
        
        // 计算弧线终点位置（稍微偏移）
        float randomOffsetX = Random.Range(-1.5f, 1.5f);  // 随机X偏移（带小数点）
        endPosition = startPosition + new Vector3(randomOffsetX, 0.0f, 0.0f);

        // 设置悬浮频率
        floatFrequency = floatSpeed;

        // 开始弧线运动
        StartCoroutine(ArcMovement(popForce));
    }
    
    // 弧线运动协程
    IEnumerator ArcMovement(float popForce)
    {
        // 给物品一个初始向上的力
        if (rb != null)
        {
            Vector2 initialForce = new Vector2(
                Random.Range(-0.7f, 0.7f),  // 随机X方向力（带小数点）
                1.0f                        // Y方向力
            ).normalized * popForce;
            
            rb.AddForce(initialForce, ForceMode2D.Impulse);
            
            // 给物品一个旋转力
            float randomTorque = Random.Range(-5.0f, 5.0f);  // 随机扭矩（带小数点）
            rb.AddTorque(randomTorque, ForceMode2D.Impulse);
        }

        // 等待弧线运动完成
        float elapsedTime = 0.0f;
        while (elapsedTime < arcDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // 计算当前高度比例
            float t = Mathf.Clamp01(elapsedTime / arcDuration);
            float height = maxHeight * Mathf.Sin(t * Mathf.PI);  // 正弦曲线高度
            
            // 更新位置（保持在X轴上，只改变Y轴高度）
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(currentPos.x, startPosition.y + height, currentPos.z);
            
            yield return null;
        }
        
        // 弧线运动结束，开始悬浮
        StartFloating();
    }

    void StartFloating()
    {
        isFloating = true;
        
        // 停止物理运动
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            rb.bodyType = RigidbodyType2D.Kinematic;  // 切换为运动学模式
        }
    }
    
    void Update()
    {
        if (isFloating&& !isAttracting)
        {
            // 检测玩家距离
            CheckPlayerDistance();
            
            // 上下悬浮效果
            float floatOffset = floatAmplitude * Mathf.Sin(Time.time * floatFrequency);
            transform.position = new Vector3(
                transform.position.x,
                endPosition.y + floatOffset,
                transform.position.z
            );

            // 旋转效果
            transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
        }
        else if (isAttracting && playerTransform != null)
        {
            // 执行吸附逻辑
            AttractToPlayer();
        }
    }
    
    void CheckPlayerDistance()
    {
        // 查找玩家对象
        GameObject player = GameManager.Instance.GetPlayer().gameObject;
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            // 如果玩家在吸附范围内
            if (distance <= attractionDistance)
            {
                StartAttraction(player.transform);
            }
        }
    }
    
    // 开始吸附
    void StartAttraction(Transform player)
    {
        isAttracting = true;
        playerTransform = player;
        
        // 吸附时的视觉效果
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0.0f;
            rb.drag = 0.2f; // 减少阻力，让吸附更流畅
        }

        // 吸附开始时的缩放效果
        StartCoroutine(AttractionScaleEffect());
    }
    
    // 吸附到玩家
    void AttractToPlayer()
    {
        if (playerTransform == null) return;

        // 计算方向向量
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        
        // 计算距离
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        
        // 根据距离调整速度（距离越近速度越快）
        float speed = Mathf.Lerp(attractionSpeed, attractionSpeed * 2.0f, 1.0f - (distance / attractionDistance));
        
        // 移动到玩家
        transform.position += direction * speed * Time.deltaTime;

        // 增加磁力效果：当距离很近时，速度更快
        if (distance < attractionDistance * 0.5f)
        {
            float magnetForce = magnetEffectIntensity * (1.0f - (distance / (attractionDistance * 0.5f)));
            transform.position += direction * magnetForce * Time.deltaTime;
        }

        // 旋转效果（吸附时旋转更快）
        transform.Rotate(0.0f, 0.0f, rotationSpeed * 1.5f * Time.deltaTime);

        // 如果非常接近玩家，直接收集
        if (distance < 0.3f)
        {
            collectAction.Invoke();
            // play sound effect
            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.itemPickup);
            Destroy(this.gameObject);
        }
    }
    
    // 吸附时的缩放动画
    IEnumerator AttractionScaleEffect()
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0.0f;
        float duration = 0.2f;

        // 缩放动画
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            // 先缩小后恢复
            float scale = Mathf.SmoothStep(1.0f, 0.8f, t) * Mathf.SmoothStep(0.8f, 1.2f, t);
            transform.localScale = originalScale * scale;
            
            yield return null;
        }

        // 恢复原始大小
        transform.localScale = originalScale * 1.1f; // 稍微大一点，突出效果
    }
}
