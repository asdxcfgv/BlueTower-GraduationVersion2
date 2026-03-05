using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnums
{
    public enum Orientation
    {
        north,
        east,
        south,
        west,
        none
    }
    
    public enum AimDirection
    {
        Up,
        Right,
        Left,
        Down
    }
    
    public enum ChestSpawnEvent
    {
        onRoomEntry,
        onEnemiesDefeated
    }

    public enum ChestSpawnPosition
    {
        atSpawnerPosition,
        atPlayerPosition
    }

    public enum ChestState
    {
        closed,
        healthItem,
        ammoItem,
        weaponItem,
        empty
    }
    
    public enum GameState
    {
        initializeGame,
        gameStarted,
        playingLevel,
        engagingEnemies,
        bossStage,
        engagingBoss,
        levelCompleted,
        gameWon,
        gameLost,
        gamePaused,
        dungeonOverviewMap,
        restartGame
    }
}
