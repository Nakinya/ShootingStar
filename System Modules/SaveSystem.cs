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
        var json = JsonUtility.ToJson(data);//��data�������ת��Ϊjson��ʽ�ַ���
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
    public static string LoadFromPlayerPrefs(string key)
    {
        return PlayerPrefs.GetString(key, null);//�ڶ�������ΪĬ��ֵ,keyΪ��ʱ�򷵻صڶ�������
    }
    #endregion
    #region SaveByJson
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//Application.persistentDataPath���API�ṩ�����洢�������ݵ�·����
                                                                              //���һ���ݴ��������ƽ̨�Զ����
        try
        {
            File.WriteAllText(path, json);//���ļ���д�����ݣ�ͬ���Ḳ�ǣ� ·�������ļ���
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
    public static bool SaveFileExists(string fileName)//�ж��ļ��Ƿ����
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(path);
    }

    #endregion
}
