using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
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
    public AnimatorController heartAnimator;
    #region Tooltip
    [Tooltip("Populate with bullet icon sprite")]
    #endregion
    public Sprite bulletIcon;
    public AnimatorController bulletAnimator;
    
    
    
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
