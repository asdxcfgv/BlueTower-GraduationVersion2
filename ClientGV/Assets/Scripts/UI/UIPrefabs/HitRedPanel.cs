using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

	public class HitRedPanel : MonoBehaviour
	{
		[Header("受击红色遮罩设置")]
		[Tooltip("最高透明度")]
		[Range(0, 1)] [SerializeField]private float maxAlpha = 0.3f;
		[Tooltip("呼吸快慢")]
		[SerializeField]private float breatheSpeed = 3f;
		[Tooltip("持续时间")]
		[SerializeField]private float effectDuration = 0.8f;
		
		[Header("Cinemachine 抖动")]
		[Tooltip("抖动时间")]
		[SerializeField]
		private float shakePower = 1f;  // 抖动强度
		
		private float _effectTimer;
		private Image hitRedImage;
		private bool onOpen = false;

		private void Awake()
		{
			hitRedImage= this.GetComponentInChildren<Image>();
		}

		private void OnEnable()
		{
			hitRedImage.color = new Color(1, 0, 0, 0);
			_effectTimer = 0f;
			onOpen = true;
			transform.SetAsFirstSibling();
			if (GameManager.Instance.GetPlayer().GetComponent<CinemachineImpulseSource>() != null)
			{
				GameManager.Instance.GetPlayer().GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(shakePower);
			}
		}
		
		
		void Update()
		{
			// 红色遮罩淡入淡出
			UpdateHitRedEffect();
		}
		
		void UpdateHitRedEffect()
		{
			if(!onOpen)
				return;
			if (_effectTimer <=effectDuration)
			{
				_effectTimer += Time.deltaTime;

				// 呼吸曲线：正弦波循环闪烁
				float alpha = Mathf.Sin(_effectTimer * breatheSpeed) * maxAlpha;
				// 保证不会负透明度
				alpha = Mathf.Abs(alpha);
				// 随时间整体衰减
				alpha *= Mathf.Clamp01((effectDuration-_effectTimer) / effectDuration);

				hitRedImage.color = new Color(1, 0, 0, alpha);
			}
			else
			{
				hitRedImage.color = new Color(1, 0, 0, 0);
				this.gameObject.Hide();
			}
		}
	}
