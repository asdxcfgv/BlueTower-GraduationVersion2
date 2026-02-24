using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using QFramework.Example;
using UnityEngine;
using static GlobalEnums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip

    [Tooltip("Populate with the starting dungeon level for testing , first level = 0")]

    #endregion Tooltip

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private FSM<GameState> m_gameStateFSM = new FSM<GameState>();
    
    private Room currentRoom;
    
    private Room previousRoom;
    
    private PlayerDetailsSO playerDetails;
    
    private Player player;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != null)
            {
                Instance = this;
            }
        }
        
        // Set player details - saved in current player scriptable object from the main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        // Instantiate player
        InstantiatePlayer();

    }

    private void Start()
    {
        InitFSM();
    }

    private void Update()
    {
        m_gameStateFSM.Update();
        // For testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_gameStateFSM.ChangeState(GameState.gameStarted);
        }

    }
    

    private void InitFSM()
    {
        m_gameStateFSM.State(GameState.initializeGame).OnEnter(() =>
        {
            UIKit.OpenPanel<MinimapPanel>(UILevel.PopUI,null,"UIPrefabs/MinimapPanel");
            
            m_gameStateFSM.ChangeState(GameState.gameStarted);
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            
        });
        m_gameStateFSM.State(GameState.gameStarted).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            PlayDungeonLevel(currentDungeonLevelListIndex);
            
            m_gameStateFSM.ChangeState(GameState.playingLevel);
            
            
        });
        m_gameStateFSM.State(GameState.playingLevel).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {

        });
        m_gameStateFSM.StartState(GameState.initializeGame);
    }


    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }
        
        StaticEventHandler.OnRoomChanged.Trigger(new RoomChangedEventArgs(currentRoom));
        
        // Set player roughly mid-room
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // Get nearest spawn point in room nearest to player
        player.gameObject.transform.position = Global.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }
    
    /// <summary>
    /// Create player in scene at position
    /// </summary>
    private void InstantiatePlayer()
    {
        // Instantiate player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        // Initialize Player
        player = playerGameObject.GetComponent<Player>();

        player.Initialize(playerDetails);

    }
    /// <summary>
    /// Get the player
    /// </summary>
    public Player GetPlayer()
    {
        return player;
    }
    
    /// <summary>
    /// Get the player minimap icon
    /// </summary>
    public Sprite GetPlayerMiniMapIcon()
    {
        return playerDetails.playerMiniMapIcon;
    }

    /// <summary>
    /// Get the current room the player is in
    /// </summary>
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
    
    /// <summary>
    /// Set the current room the player in in
    /// </summary>
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;

        //// Debug
        //Debug.Log(room.prefab.name.ToString());
    }
}
