using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;

namespace MyBehaviourTree 
{
    public class Blackboard : MonoBehaviour
    {
        // 用字典存所有黑板数据：string 是key，object 是任意类型值
        private Dictionary<string, object> data = new Dictionary<string, object>();

        // 写数据
        public void SetValue(string key, object value)
        {
            if (data.ContainsKey(key))
                data[key] = value;
            else
                data.Add(key, value);
        }

        // 读数据（带泛型，自动转类型）
        public T GetValue<T>(string key)
        {
            if (data.ContainsKey(key) && data[key] is T value)
                return value;

            return default; // 找不到返回默认值
        }

        // 判断是否存在某个key
        public bool HasKey(string key) => data.ContainsKey(key);

        // 清空
        public void Clear() => data.Clear();
    }
    
}