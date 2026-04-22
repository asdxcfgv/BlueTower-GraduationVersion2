using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GlobalEnums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    #region Header GAMEOBJECT REFERENCES
    [Space(10)]
    [Header("GAMEOBJECT REFERENCES")]
    #endregion Header GAMEOBJECT REFERENCES

    #region Tooltip
    [Tooltip("Populate with the MessageText textmeshpro component in the FadeScreenUI")]
    #endregion Tooltip
    [SerializeField] private TextMeshProUGUI messageTextTMP;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    #region Tooltip
    [Tooltip("Populate with the FadeImage canvasgroup component in the FadeScreenUI")]
    #endregion Tooltip
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip

    [Tooltip("Populate with the starting dungeon level for testing , first level = 0")]

    #endregion Tooltip

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject hitRedPanel;
    
    
    private Room currentRoom;
    
    private Room previousRoom;
    
    private PlayerDetailsSO playerDetails;
    
    private Player player;
    
    [HideInInspector] public GameState previousGameState;
    
    private FSM<GameState> m_gameStateFSM = new FSM<GameState>();
    
    private bool isFading = false;
    
    private InstantiatedRoom bossRoom;
    
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
        
        ResKit.Init();
        
        // Set player details - saved in current player scriptable object from the main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        // Instantiate player
        InstantiatePlayer();

    }
    
    private void OnEnable()
    {
        // Subscribe to room changed event.
        StaticEventHandler.OnRoomChanged.Register(StaticEventHandler_OnRoomChanged);
        
        // Subscribe to room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated.Register(StaticEventHandler_OnRoomEnemiesDefeated);
        
        // Subscribe to player destroyed event
        player.destroyedEvent.OnDestroyed.Register(Player_OnDestroyed);
    }

    private void OnDisable()
    {
        // Unsubscribe from room changed event
        StaticEventHandler.OnRoomChanged.UnRegister(StaticEventHandler_OnRoomChanged);

        // Subscribe to room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated.UnRegister(StaticEventHandler_OnRoomEnemiesDefeated);
        
        // Unubscribe from player destroyed event
        player.destroyedEvent.OnDestroyed.UnRegister(Player_OnDestroyed);
    }

    /// <summary>
    /// Handle room changed event
    /// </summary>
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }
    
    /// <summary>
    /// Handle room enemies defeated event
    /// </summary>
    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        RoomEnemiesDefeated();
    }

    /// <summary>
    /// Handle player destroyed event
    /// </summary>
    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        previousGameState = m_gameStateFSM.CurrentStateId;
        m_gameStateFSM.ChangeState(GameState.gameLost);
    }
    
    private void Start()
    {
        InitFSM();
    }

    private void Update()
    {
        m_gameStateFSM.Update();
    }
    

    private void InitFSM()
    {
        m_gameStateFSM.State(GameState.initializeGame).OnEnter(() =>
        {
            previousGameState = GameState.gameStarted;
            pauseMenu.Hide();
            m_gameStateFSM.ChangeState(GameState.gameStarted);
            
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            
        });
        m_gameStateFSM.State(GameState.gameStarted).OnEnter(() =>
        {
            // Play first level
            PlayDungeonLevel(currentDungeonLevelListIndex);

            // Trigger room enemies defeated since we start in the entrance where there are no enemies (just in case you have a level with just a boss room!)
            RoomEnemiesDefeated();
            
            m_gameStateFSM.ChangeState(GameState.playingLevel);
            
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {

        });
        m_gameStateFSM.State(GameState.playingLevel).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGameMenu();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                DisplayDungeonOverviewMap();
            }
        });
        m_gameStateFSM.State(GameState.levelCompleted).OnEnter(() =>
        {
            // Display level completed text
            StartCoroutine(LevelCompleted());
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            
        });
        m_gameStateFSM.State(GameState.gameWon).OnEnter(() =>
        {
            if (previousGameState != GameState.gameWon)
                StartCoroutine(GameWon());
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            
        });
        m_gameStateFSM.State(GameState.gameLost).OnEnter(() =>
        {
            if (previousGameState != GameState.gameLost)
            {
                StopAllCoroutines(); // Prevent messages if you clear the level just as you get killed
                StartCoroutine(GameLost());
            }
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            
        });
        m_gameStateFSM.State(GameState.restartGame).OnEnter(() =>
        {
            RestartGame();
            
        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            
        });
        m_gameStateFSM.State(GameState.dungeonOverviewMap).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            if (Input.GetKeyUp(KeyCode.M))
            {
                // Clear dungeonOverviewMap
                DungeonMap.Instance.ClearDungeonOverViewMap();
            }
        });
        m_gameStateFSM.State(GameState.bossStage).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGameMenu();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                DisplayDungeonOverviewMap();
            }
        });
        m_gameStateFSM.State(GameState.engagingEnemies).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGameMenu();
            }
        });
        m_gameStateFSM.State(GameState.engagingBoss).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGameMenu();
            }
        });
        m_gameStateFSM.State(GameState.gamePaused).OnEnter(() =>
        {

        }).OnExit(() =>
        {

        }).OnUpdate(() =>
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGameMenu();
            }
        });
        m_gameStateFSM.StartState(GameState.initializeGame);
    }
    
    public void PauseGameMenu()
    {
        if (m_gameStateFSM.CurrentStateId != GameState.gamePaused)
        {
            pauseMenu.Show();
            GetPlayer().playerControl.DisablePlayer();

            // Set game state
            previousGameState = m_gameStateFSM.CurrentStateId;
            m_gameStateFSM.ChangeState(GameState.gamePaused);
        }
        else if (m_gameStateFSM.CurrentStateId == GameState.gamePaused)
        {
            pauseMenu.Hide();
            GetPlayer().playerControl.EnablePlayer();

            // Set game state
            m_gameStateFSM.ChangeState(previousGameState);
            previousGameState = GameState.gamePaused;

        }
    }

    private void DisplayDungeonOverviewMap()
    {
        // return if fading
        if (isFading)
            return;

        // Display dungeonOverviewMap
        DungeonMap.Instance.DisplayDungeonOverViewMap();
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
        
        // Display Dungeon Level Text
        StartCoroutine(DisplayDungeonLevelText());
    }
    
    /// <summary>
    /// Display the dungeon level text
    /// </summary>
    private IEnumerator DisplayDungeonLevelText()
    {
        // Set screen to black
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));

        GetPlayer().playerControl.DisablePlayer();

        string messageText = dungeonLevelList[currentDungeonLevelListIndex].levelName.ToUpper();

        yield return StartCoroutine(DisplayMessageRoutine(messageText, Color.white, 2f));

        GetPlayer().playerControl.EnablePlayer();

        // Fade In
        yield return StartCoroutine(Fade(1f, 0f, 2f, Color.black));

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
    /// Restart the game
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
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
    
    /// <summary>
    /// Room enemies defated - test if all dungeon rooms have been cleared of enemies - if so load
    /// next dungeon game level
    /// </summary>
    private void RoomEnemiesDefeated()
    {
        // Initialise dungeon as being cleared - but then test each room
        bool isDungeonClearOfRegularEnemies = true;
        bossRoom = null;

        // Loop through all dungeon rooms to see if cleared of enemies
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            // skip boss room for time being
            if (keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

            // check if other rooms have been cleared of enemies
            if (!keyValuePair.Value.isClearedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        // Set game state
        // If dungeon level completly cleared (i.e. dungeon cleared apart from boss and there is no boss room OR dungeon cleared apart from boss and boss room is also cleared)
        if ((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isClearedOfEnemies))
        {
            // Are there more dungeon levels then
            if (currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                m_gameStateFSM.ChangeState(GameState.levelCompleted);
            }
            else
            {
                m_gameStateFSM.ChangeState(GameState.gameWon);
            }
        }
        // Else if dungeon level cleared apart from boss room
        else if (isDungeonClearOfRegularEnemies)
        {
            m_gameStateFSM.ChangeState(GameState.bossStage);

            StartCoroutine(BossStage());
        }

    }
    
    /// <summary>
    /// Display the message text for displaySeconds  - if displaySeconds =0 then the message is displayed until the return key is pressed
    /// </summary>
    private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {
        // Set text
        messageTextTMP.SetText(text);
        messageTextTMP.color = textColor;

        // Display the message for the given time
        if (displaySeconds > 0f)
        {
            float timer = displaySeconds;

            while (timer > 0f && !Input.GetKeyDown(KeyCode.Return))
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else
            // else display the message until the return button is pressed
        {
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }

        yield return null;

        // Clear text
        messageTextTMP.SetText("");
    }
    
    /// <summary>
    /// Enter boss stage
    /// </summary>
    private IEnumerator BossStage()
    {
        // Activate boss room
        bossRoom.gameObject.SetActive(true);

        // Unlock boss room
        bossRoom.UnlockDoors(0f);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Fade in canvas to display text message
        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // Display boss message
        yield return StartCoroutine(DisplayMessageRoutine("干得好！你坚持了下来\n\n接下来，你将面临最终的考验!", Color.white, 5f));

        // Fade out canvas
        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

    }
    
    /// <summary>
    /// Fade Canvas Group
    /// </summary>
    public IEnumerator Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
    {
        isFading = true;
        
        Image image = canvasGroup.GetComponent<Image>();
        image.color = backgroundColor;

        float time = 0;

        while (time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
            yield return null;
        }

        isFading = false;
    }
    
    /// <summary>
    /// Show level as being completed - load next level
    /// </summary>
    private IEnumerator LevelCompleted()
    {
        // Play next level
        m_gameStateFSM.ChangeState(GameState.playingLevel);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Fade in canvas to display text message
        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // Display level completed
        yield return StartCoroutine(DisplayMessageRoutine("干得好！\n\n你成功通过了这一层！", Color.white, 5f));

        yield return StartCoroutine(DisplayMessageRoutine("收集你的资源....按下回车键\n\n前往地牢的更深处", Color.white, 5f));

        // Fade out canvas
        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // When player presses the return key proceed to the next level
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        yield return null; // to avoid enter being detected twice

        // Increase index to next level
        currentDungeonLevelListIndex++;

        PlayDungeonLevel(currentDungeonLevelListIndex);
    }
    
    /// <summary>
    /// Game Won
    /// </summary>
    private IEnumerator GameWon()
    {
        previousGameState = GameState.gameWon;

        // Disable player
        GetPlayer().playerControl.DisablePlayer();

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        // Display game won
        yield return StartCoroutine(DisplayMessageRoutine("干得好！你成功到达了塔顶！", Color.white, 3f));

        //yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED " + gameScore.ToString("###,###0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("按回车键重启游戏", Color.white, 0f));
        
        // Set game state to restart game
        m_gameStateFSM.ChangeState(GameState.restartGame);
    }

    /// <summary>
    /// Game Lost
    /// </summary>
    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        // Disable player
        GetPlayer().playerControl.DisablePlayer();

        // Wait 1 seconds
        yield return new WaitForSeconds(1f);

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        // Disable enemies (FindObjectsOfType is resource hungry - but ok to use in this end of game situation)
        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        // Display game lost
        yield return StartCoroutine(DisplayMessageRoutine("YOU ARE DEAD", Color.white, 2f));

        //yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED " + gameScore.ToString("###,###0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("按下回车键重启游戏", Color.white, 0f));
        
        // Set game state to restart game
        m_gameStateFSM.ChangeState(GameState.restartGame);
    }
    
    /// <summary>
    /// Get the current dungeon level
    /// </summary>
    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    public GameState GetCurrentGameState()
    {
        return m_gameStateFSM.CurrentStateId;
    }

    public CinemachineVirtualCamera GetCamera()
    {
        return virtualCamera;
    }

    public void ChangeState(GameState newState)
    {
        m_gameStateFSM.ChangeState(newState);
    }

    public void GetHit()
    {
        if (!hitRedPanel.activeSelf)
        {
            hitRedPanel.Show();
        }
    }
}
