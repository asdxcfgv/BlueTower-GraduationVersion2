using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFramework;
using UnityEditor.Animations;
using UnityEngine;
using static GlobalEnums;

public  class Global : Architecture<Global>
{
    public static Camera mainCamera;
    
    #region FIRING CONTROL
    public const float useAimAngleDistance = 3.5f; // if the target distance is less than this then the aim angle will be used (calculated from player), else the weapon aim angle will be used (calculated from the weapon). 
    #endregion

    #region UI PARAMETERS
    public const float uiAmmoIconSpacing = 4f;
    #endregion
    
    #region UNITS
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion
    
    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion
    
    #region ROOM SETTINGS
    public const float fadeInTime = 0.5f; // time to fade in the room
    public const int maxChildCorridors = 3; // Max number of child corridors leading from a room. - maximum should be 3 although this is not recommended since it can cause the dungeon building to fail since the rooms are more likely to not fit together;
    public const float doorUnlockDelay = 1f;
    #endregion
    
    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion
    
    #region ASTAR PATHFINDING PARAMETERS
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const int targetFrameRateToSpreadPathfindingOver = 60;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;

    #endregion
    
    #region ANIMATOR PARAMETERS
    public static int aimUp = Animator.StringToHash("aimUp");
    
    public static int aimDown = Animator.StringToHash("aimDown");
    
    public static int aimRight = Animator.StringToHash("aimRight");
    
    public static int aimLeft = Animator.StringToHash("aimLeft");
    
    public static int isIdle = Animator.StringToHash("isIdle");
    
    public static int isMoving = Animator.StringToHash("isMoving");
    
    public static int isAttacking = Animator.StringToHash("isAttacking");
    
    // Animator parameters - Door
    public static int open = Animator.StringToHash("open");
    
    public static float baseSpeedForPlayerAnimations = 8f;
    
    // Animator parameters - Enemy
    public static float baseSpeedForEnemyAnimations = 3f;

    #endregion
    
    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion
    
    protected override void Init()
    {
        
    }

    #region 工具方法
    /// <summary>
    /// 根据表格名称读取StreamingAssets/Excel下的Excel文件
    /// </summary>
    /// <param name="excelName">Excel文件名（不含扩展名）</param>
    /// <returns>二维字符串数组，string[行][列]，如果读取失败返回null</returns>
    public static string[][] ReadExcelData(string excelName)
    {
        // 构建Excel文件路径
        string excelPath = Path.Combine(Application.streamingAssetsPath, "Excel", excelName + ".xlsx");
            
        // 如果.xlsx不存在，尝试.xls
        if (!File.Exists(excelPath))
        {
            excelPath = Path.Combine(Application.streamingAssetsPath, "Excel", excelName + ".xls");
        }

        // 检查文件是否存在
        if (!File.Exists(excelPath))
        { 
            Debug.LogError($"Excel文件不存在: {excelPath}");
            return null;
        }

        try
        {
            using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = global::ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                {
                    // 读取所有数据到List
                    List<string[]> dataList = new List<string[]>();

                    // 逐行读取
                    while (reader.Read())
                    {
                        int columnCount = reader.FieldCount;
                        string[] rowData = new string[columnCount];

                        for (int j = 0; j < columnCount; j++)
                        {
                            // 读取单元格数据并转换为字符串
                            var cellValue = reader.GetValue(j);
                            rowData[j] = cellValue?.ToString() ?? string.Empty;
                        }

                        dataList.Add(rowData);
                    }

                    // 转换为二维数组
                    string[][] result = dataList.ToArray();
                        
                    Debug.Log($"成功读取Excel: {excelName}, 共 {result.Length} 行");
                    return result;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取Excel文件失败: {excelName}\n错误信息: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Get the direction vector from an angle in degrees
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }
    
    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// list empty or contains null value check - returns true if there is an error
    /// </summary>
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
            return true;
        }


        foreach (var item in enumerableObjectToCheck)
        {

            if (item == null)
            {
                Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }
    
    /// <summary>
    /// Get the mouse world position.
    /// </summary>
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseScreenPosition = Input.mousePosition;

        // Clamp mouse position to screen size
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        worldPosition.z = 0f;

        return worldPosition;

    }

    /// <summary>
    /// Get the angle in degrees from a direction vector
    /// </summary>
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }

    /// <summary>
    /// Get AimDirection enum value from the pased in angleDegrees
    /// </summary>
    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDirection;

        // Set player direction

        // Up
        if (angleDegrees > 45f && angleDegrees <= 135f)
        {
            aimDirection = AimDirection.Up;
        }
        // Left
        else if ((angleDegrees <= 180f && angleDegrees > 135f) || (angleDegrees > -180 && angleDegrees <= -135f))
        {
            aimDirection = AimDirection.Left;
        }
        // Down
        else if ((angleDegrees > -135f && angleDegrees <= -45f))
        {
            aimDirection = AimDirection.Down;
        }
        // Right
        else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0 && angleDegrees <= 45f))
        {
            aimDirection = AimDirection.Right;
        }
        else
        {
            aimDirection = AimDirection.Right;
        }

        return aimDirection;

    }

    public static float GetAnimationClipLengthBySuffix(Animator animator, string clipName)
    {
        // 获取所有动画片段
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        
        // 遍历所有动画片段
        foreach (AnimationClip clip in clips)
        {
            // 如果找到了需要的动画片段
            if (clip.name.Contains(clipName))
            {
                // 获取并返回动画时长
                return clip.length;
            }
        }

        return 0f;
    }
    
    
    /// <summary>
    /// Get the nearest spawn position to the player
    /// </summary>
    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f);

        // Loop through room spawn positions
        foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            // convert the spawn grid positions to world positions
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                nearestSpawnPosition = spawnPositionWorld;
            }
        }

        return nearestSpawnPosition;

    }
    #endregion
        
        
}
