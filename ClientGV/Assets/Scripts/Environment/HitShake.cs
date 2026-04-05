using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitShake : MonoBehaviour
{
    [Header("抖动强度")]
    [SerializeField]private float shakePower = 0.2f;

    [Header("抖动时长（秒）")]
    [SerializeField]private float shakeTime = 0.1f;

    private Vector3 _originalPos; // 原始位置
    private bool _isShaking = false;
    // Start is called before the first frame update
    /// <summary>
    /// 外部调用：触发抖动
    /// </summary>
    public void PlayShake()
    {
        if (!_isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        _isShaking = true;
        _originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < shakeTime)
        {
            // 随机偏移位置 = 抖动效果
            Vector3 randomPos = _originalPos + Random.insideUnitSphere * shakePower;
            transform.localPosition = randomPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 抖动结束，还原位置
        transform.localPosition = _originalPos;
        _isShaking = false;
    }
}
