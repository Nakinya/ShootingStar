using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilesPools;
    [SerializeField] Pool[] VFXPools;
    [SerializeField] Pool[] lootItemPools;
    static Dictionary<GameObject, Pool> dictionary;
    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilesPools);
        Initialize(VFXPools);
        Initialize(lootItemPools);
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilesPools);
        CheckPoolSize(VFXPools);
        CheckPoolSize(lootItemPools);
    }
#endif
    void CheckPoolSize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("Pool:{0} has a runtime size {1} bigger than size {2}", 
                    pool.Prefab.name, 
                    pool.RuntimeSize, 
                    pool.Size));
            }
        }
    }
    void Initialize(Pool[] pools)//初始化对象池组
    {
        foreach (Pool pool in pools)
        {
#if UNITY_EDITOR //条件编译指令，框起来的代码只会在指定平台上才编译
            if (dictionary.ContainsKey(pool.Prefab))//防止key重复
            {
                Debug.LogError("same prefab in multiple pools.Prefab:"+pool.Prefab.name);
                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("pool: " + pool.Prefab.name).transform;
            poolParent.parent = this.transform;
            pool.Initialize(poolParent);
        }
    }
    /// <summary>
    /// 根据传入的参数返回对象池中准备好的对象
    /// </summary>
    /// <param name="prefab">指定的游戏对象预制体</param>
    /// <returns>对象池中预备好的游戏对象</returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find the prefab. Prefab:"+prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }
    /// <summary>
    /// 根据传入的参数返回对象池中准备好的对象
    /// </summary>
    /// <param name="prefab">指定的游戏对象预制体</param>
    /// <param name="position">生成位置</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find the prefab. Prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }
    /// <summary>
    /// 根据传入的参数返回对象池中准备好的对象
    /// </summary>
    /// <param name="prefab">指定的游戏对象预制体</param>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">生成旋转值</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find the prefab. Prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position,rotation);
    }
    /// <summary>
    /// 根据传入的参数返回对象池中准备好的对象
    /// </summary>
    /// <param name="prefab">指定的游戏对象预制体</param>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">生成旋转值</param>
    /// <param name="localScale">生成缩放值</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool manager could not find the prefab. Prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation,localScale);    
    }
}
