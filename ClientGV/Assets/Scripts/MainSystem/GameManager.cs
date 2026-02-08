using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using static GlobalEnums;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip

    [Tooltip("Populate with the starting dungeon level for testing , first level = 0")]

    #endregion Tooltip

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private FSM<GameState> m_gameStateFSM = new FSM<GameState>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                instance = this;
            }
        }
    }

    private void Start()
    {
        InitFSM();
    }

    private void Update()
    {
        m_gameStateFSM.Update();
        // For testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_gameStateFSM.ChangeState(GameState.gameStarted);
        }

    }
    

    private void InitFSM()
    {
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
        m_gameStateFSM.StartState(GameState.gameStarted);
    }


    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }


    }

}
