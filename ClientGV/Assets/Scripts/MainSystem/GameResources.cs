using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    public RoomNodeTypeListSO roomNodeTypeList;
    
    #region Header PLAYER
    [Space(10)]
    [Header("PLAYER")]
    #endregion Header PLAYER
    #region Tooltip
    [Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
    #endregion Tooltip
    public CurrentPlayerSO currentPlayer;


    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    #region Tooltip
    [Tooltip("Dimmed Material")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("Sprite-Lit-Default Material")]
    #endregion
    public Material litMaterial;

    #region Tooltip
    [Tooltip("Populate with the Variable Lit Shader")]
    #endregion
    public Shader variableLitShader;
    
    #region Tooltip
    [Tooltip("Populate with the Materialize Shader")]
    #endregion
    public Shader materializeShader;
    
    #region Header CHESTS
    [Space(10)]
    [Header("CHESTS")]
    #endregion
    #region Tooltip
    [Tooltip("Chest item prefab")]
    #endregion
    public GameObject chestItemPrefab;
    #region Tooltip
    [Tooltip("Populate with heart icon sprite")]
    #endregion
    public Sprite heartIcon;
    public RuntimeAnimatorController heartAnimator;
    #region Tooltip
    [Tooltip("Populate with bullet icon sprite")]
    #endregion
    public Sprite normalBulletIcon;
    public RuntimeAnimatorController normalBulletAnimator;

    public Sprite electronBulletIcon;
    public RuntimeAnimatorController electronBulletAnimator;

    public Sprite boomBulletIcon;
    public RuntimeAnimatorController boomBulletAnimator;

    public GameObject minimapBossIconPrefab;
    
    #region Header SOUNDS
    [Space(10)]
    [Header("SOUNDS")]
    #endregion Header
    #region Tooltip
    [Tooltip("Populate with the sounds master mixer group")]
    #endregion
    public AudioMixerGroup soundsMasterMixerGroup;
    #region Tooltip
    [Tooltip("开关门声音")]
    #endregion Tooltip
    public SoundEffectSO doorOpenCloseSoundEffect;
    #region Tooltip
    [Tooltip("宝箱打开声音")]
    #endregion
    public SoundEffectSO chestOpen;
    #region Tooltip
    [Tooltip("物品拾起声音")]
    #endregion
    public SoundEffectSO itemPickup;

    
    
    #region Header MUSIC
    [Space(10)]
    [Header("MUSIC")]
    #endregion Header MUSIC
    #region Tooltip
    [Tooltip("Populate with the music master mixer group")]
    #endregion
    public AudioMixerGroup musicMasterMixerGroup;
    #region Tooltip
    [Tooltip("music on full snapshot")]
    #endregion Tooltip
    public AudioMixerSnapshot musicOnFullSnapshot;
    #region Tooltip
    [Tooltip("music low snapshot")]
    #endregion Tooltip
    public AudioMixerSnapshot musicLowSnapshot;
    #region Tooltip
    [Tooltip("music off snapshot")]
    #endregion Tooltip
    public AudioMixerSnapshot musicOffSnapshot;
    
    #region Header SPECIAL TILEMAP TILES
    [Space(10)]
    [Header("SPECIAL TILEMAP TILES")]
    #endregion Header SPECIAL TILEMAP TILES
    #region Tooltip
    [Tooltip("Collision tiles that the enemies can navigate to")]
    #endregion Tooltip
    public TileBase[] enemyUnwalkableCollisionTilesArray;
    #region Tooltip
    [Tooltip("Preferred path tile for enemy navigation")]
    #endregion Tooltip
    public TileBase preferredEnemyPathTile;
}
