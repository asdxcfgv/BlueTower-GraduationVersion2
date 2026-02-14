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
        UpRight,
        UpLeft,
        Right,
        Left,
        Down
    }
    
    public enum GameState
    {
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
