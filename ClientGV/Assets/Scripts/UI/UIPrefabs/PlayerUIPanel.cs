using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;
using QFramework;
using TMPro;

	public partial class PlayerUIPanel : MonoBehaviour
	{
		[SerializeField]
		private Image Gun_Sprite;
		[SerializeField]
		private GameObject BulletIcon_Content;
		[SerializeField]
		private GameObject BulletIcon_Pre;
		[SerializeField]
		private GameObject HeartIcon_Content;
		[SerializeField]
		private GameObject HeartIcon_Pre;
		[SerializeField]
		private TextMeshProUGUI NormalBulletText;
		[SerializeField]
		private TextMeshProUGUI ElectronBulletText;
		[SerializeField]
		private TextMeshProUGUI BoomBulletText;
		[SerializeField] private Sprite disableHealthSprite;
		[SerializeField] private Sprite enableHealthSprite;
		[SerializeField] private Sprite disableAmmoSprite;
		[SerializeField] private Sprite enableAmmoSprite;
		[SerializeField] private int ammoSpawnNum;
		[SerializeField] private int heartSpawnNum;
		private List<GameObject> ammoIconList = new List<GameObject>();
		private List<GameObject> healthHeartsList = new List<GameObject>();
		private Player player;

		private void Awake()
		{
			// Get player
			player = GameManager.Instance.GetPlayer();
			
			healthHeartsList.Clear();
			
			ammoIconList.Clear();
			

			for (int i = 0; i < heartSpawnNum; i++)
			{
				healthHeartsList.Add(Instantiate(HeartIcon_Pre,HeartIcon_Content.transform));
			}

			HeartIcon_Pre.Hide();

			for (int i = 0; i < ammoSpawnNum; i++)
			{
				ammoIconList.Add(Instantiate(BulletIcon_Pre,BulletIcon_Content.transform));
			}

			BulletIcon_Pre.Hide();
			
			
			// Update active weapon status on the UI
			SetActiveWeapon(player.activeWeapon.GetCurrentWeapon());
			
			AmmoReloaded();
		}

		private void Start()
		{
			
		}

		private void OnEnable()
		{
			// Subscribe to set active weapon event
			player.setActiveWeaponEvent.OnSetActiveWeapon.Register(SetActiveWeaponEvent_OnSetActiveWeapon);

			// Subscribe to weapon fired event
			player.weaponFiredEvent.OnWeaponFired.Register(WeaponFiredEvent_OnWeaponFired);
			

			// Subscribe to weapon reloaded event
			player.weaponReloadedEvent.OnWeaponReloaded.Register(WeaponReloadedEvent_OnWeaponReloaded);
			
			player.healthEvent.OnHealthChanged.Register(HealthEvent_OnHealthChanged);

			player.playerResourcesChangedEvent.OnPlayerResourcesChanged.Register(PlayerResourcesChangedEvent_OnPlayerResourcesChanged);
		}

		private void OnDisable()
		{
			// Subscribe to set active weapon event
			player.setActiveWeaponEvent.OnSetActiveWeapon.UnRegister(SetActiveWeaponEvent_OnSetActiveWeapon);

			// Subscribe to weapon fired event
			player.weaponFiredEvent.OnWeaponFired.UnRegister(WeaponFiredEvent_OnWeaponFired);

			// Subscribe to weapon reloaded event
			player.weaponReloadedEvent.OnWeaponReloaded.UnRegister(WeaponReloadedEvent_OnWeaponReloaded);
			
			player.healthEvent.OnHealthChanged.UnRegister(HealthEvent_OnHealthChanged);
			
			player.playerResourcesChangedEvent.OnPlayerResourcesChanged.UnRegister(PlayerResourcesChangedEvent_OnPlayerResourcesChanged);
		}

		private void HealthEvent_OnHealthChanged(HealthEventArgs healthEventArgs)
		{
			SetHealthBar(healthEventArgs);
		}
		/// <summary>
		/// Handle set active weapon event on the UI
		/// </summary>
		private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEventArgs setActiveWeaponEventArgs)
		{
			SetActiveWeapon(setActiveWeaponEventArgs.weapon);
		}
		/// <summary>
		/// Handle Weapon fired event on the UI
		/// </summary>
		private void WeaponFiredEvent_OnWeaponFired(WeaponFiredEventArgs weaponFiredEventArgs)
		{
			WeaponFired(weaponFiredEventArgs.weapon);
		}
		/// <summary>
		/// Handle weapon reloaded event on the UI
		/// </summary>
		private void WeaponReloadedEvent_OnWeaponReloaded(WeaponReloadedEventArgs weaponReloadedEventArgs)
		{
			WeaponReloaded(weaponReloadedEventArgs.weapon);
		}

		private void PlayerResourcesChangedEvent_OnPlayerResourcesChanged(
			PlayerResourcesChangedEventArgs playerResourcesChangedEventArgs)
		{
			AmmoReloaded();
		}
		/// <summary>
		/// Set the active weapon on the UI
		/// </summary>
		private void SetActiveWeapon(Weapon weapon)
		{
			UpdateActiveWeaponImage(weapon.weaponDetails);
			UpdateAmmoLoadedIcons(weapon);
			Color m_Color = new Color(86f/255f, 86f/255f, 86f/255f);
			switch (weapon.weaponDetails.usingBulletType)
			{
				case BulletType.none:
					NormalBulletText.color = m_Color;
					BoomBulletText.color = m_Color;
					ElectronBulletText.color = m_Color;
					break;
				case BulletType.normal:
					NormalBulletText.color = Color.white;
					BoomBulletText.color = m_Color;
					ElectronBulletText.color = m_Color;
					break;
				case BulletType.electron:
					NormalBulletText.color = m_Color;
					BoomBulletText.color = m_Color;
					ElectronBulletText.color = Color.white;
					break;
				case BulletType.boom:
					NormalBulletText.color = m_Color;
					BoomBulletText.color = Color.white;
					ElectronBulletText.color = m_Color;
					break;
				default:
					break;
			}
		}
		
		/// <summary>
		/// Weapon fired update UI
		/// </summary>
		private void WeaponFired(Weapon weapon)
		{
			UpdateAmmoLoadedIcons(weapon);
		}
		
		/// <summary>
		/// Weapon has been reloaded - update UI if current weapon
		/// </summary>
		private void WeaponReloaded(Weapon weapon)
		{
			// if weapon reloaded is the current weapon
			if (player.activeWeapon.GetCurrentWeapon() == weapon)
			{
				UpdateAmmoLoadedIcons(weapon);
			}
		}

		private void AmmoReloaded()
		{
			int num = player.GetAmmoNum(GlobalEnums.BulletType.normal);
			NormalBulletText.text = player.GetAmmoNum(GlobalEnums.BulletType.normal).ToString();
			ElectronBulletText.text = player.GetAmmoNum(GlobalEnums.BulletType.electron).ToString();
			BoomBulletText.text = player.GetAmmoNum(GlobalEnums.BulletType.boom).ToString();
		}
		
		/// <summary>
		/// Update ammo clip icons on the UI
		/// </summary>
		private void UpdateAmmoLoadedIcons(Weapon weapon)
		{
			ClearAmmoLoadedIcons();
			float myWeaponClipRemainingAmmo = weapon.weaponClipRemainingAmmo;
			float myWeaponClipAmmoCapacity = weapon.weaponDetails.weaponClipAmmoCapacity;

			// Instantiate heart image prefabs
			int ammo = Mathf.CeilToInt((myWeaponClipRemainingAmmo/myWeaponClipAmmoCapacity) * 100f / 5f);
			
			for (int i = 0; i < ammo; i++)
			{
				ammoIconList[i].GetComponent<Image>().sprite = enableAmmoSprite;
			}
		}
		/// <summary>
		/// Clear ammo icons
		/// </summary>
		private void ClearAmmoLoadedIcons()
		{
			// Loop through icon gameobjects and destroy
			foreach (GameObject ammoIcon in ammoIconList)
			{
				ammoIcon.GetComponent<Image>().sprite = disableAmmoSprite;
			}
			
		}
		
		/// <summary>
		/// Populate active weapon image
		/// </summary>
		private void UpdateActiveWeaponImage(WeaponDetailsSO weaponDetails)
		{
			Gun_Sprite.sprite = weaponDetails.weaponSprite;
		}
		
		private void ClearHealthBar()
		{
			foreach (GameObject heartIcon in healthHeartsList)
			{
				heartIcon.GetComponent<Image>().sprite = disableHealthSprite;
			}
			
		}
		
		private void SetHealthBar(HealthEventArgs healthEventArgs)
		{
			ClearHealthBar();

			// Instantiate heart image prefabs
			int healthHearts = Mathf.CeilToInt(healthEventArgs.healthPercent * 100f / 10f);

			for (int i = 0; i < healthHearts; i++)
			{
				healthHeartsList[i].GetComponent<Image>().sprite = enableHealthSprite;
			}

		}
	}
