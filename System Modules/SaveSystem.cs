using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

///<summary>
///
///</summary>
public static class SaveSystem
{
    #region SaveByPlayerPrefs
    public static void SaveByPlayerPrefs(string key, object data)
    {
        var json = JsonUtility.ToJson(data);//将data里的数据转化为json格式字符串
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
    public static string LoadFromPlayerPrefs(string key)
    {
        return PlayerPrefs.GetString(key, null);//第二个参数为默认值,key为空时则返回第二个参数
    }
    #endregion
    #region SaveByJson
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//Application.persistentDataPath这个API提供用来存储永久数据的路径，
                                                                              //并且会根据打包发布的平台自动变更
        try
        {
            File.WriteAllText(path, json);//在文件中写入数据（同名会覆盖） 路径包括文件名
#if UNITY_EDITOR
            Debug.Log($"Saved data to {path}");
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Fail to save data to path:{path}. \n{exception}");
#endif
        }
    }
    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to load data from:{path},\n{exception}");
#endif
            return default;
        }
    }
    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete save data from:{path}, \n{exception}");
#endif
        }
    }
    public static bool SaveFileExists(string fileName)//判断文件是否存在
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(path);
    }

    #endregion
}
