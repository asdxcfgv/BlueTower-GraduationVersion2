using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFramework;
using UnityEngine;

public  class Global : Architecture<Global>
{
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
    #endregion
    
    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion
    
    #region ANIMATOR PARAMETERS
    

    // Animator parameters - Door
    public static int open = Animator.StringToHash("open");

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
