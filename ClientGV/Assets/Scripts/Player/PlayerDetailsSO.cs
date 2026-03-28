using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header PLAYER BASE DETAILS
    [Space(10)]
    [Header("玩家基本细节")]
    #endregion
    #region Tooltip
    [Tooltip("角色名字")]
    #endregion
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("玩家预制体")]
    #endregion
    public GameObject playerPrefab;

    #region Tooltip
    [Tooltip("玩家动画机")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header HEALTH
    [Space(10)]
    [Header("生命值")]
    #endregion
    #region Tooltip
    [Tooltip("玩家初始生命值")]
    #endregion
    public int playerHealthAmount;
    
    public bool isImmuneAfterHit = false;
    #region Tooltip
    [Tooltip("受击后免疫时间")]
    #endregion
    public float hitImmunityTime;
    
    #region Header WEAPON
    [Space(10)]
    [Header("武器")]
    #endregion
    #region Tooltip
    [Tooltip("玩家初始武器")]
    #endregion
    public WeaponDetailsSO startingWeapon;
    #region Tooltip
    [Tooltip("玩家初始武器列表")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList;

    #region Header OTHER
    [Space(10)]
    [Header("其他")]
    #endregion
    #region Tooltip
    [Tooltip("玩家图标")]
    #endregion
    public Sprite playerMiniMapIcon;


    #region Header OTHER
    [Space(10)]
    [Header("玩家初始子弹数量")]
    #endregion
    [Range(0,99)]public int playerNormalBullet;
    [Range(0,99)]public int playerElectronBullet;
    [Range(0,99)]public int playerBoomBullet;
}
