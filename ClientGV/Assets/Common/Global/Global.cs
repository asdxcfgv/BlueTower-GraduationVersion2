using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFramework;
using UnityEngine;

public partial class Global : Architecture<Global>
{
    protected override void Init()
    {
        
    }
    
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
}
