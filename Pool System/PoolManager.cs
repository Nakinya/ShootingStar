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
    void Initialize(Pool[] pools)//��ʼ���������
    {
        foreach (Pool pool in pools)
        {
#if UNITY_EDITOR //��������ָ��������Ĵ���ֻ����ָ��ƽ̨�ϲű���
            if (dictionary.ContainsKey(pool.Prefab))//��ֹkey�ظ�
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
    /// ���ݴ���Ĳ������ض������׼���õĶ���
    /// </summary>
    /// <param name="prefab">ָ������Ϸ����Ԥ����</param>
    /// <returns>�������Ԥ���õ���Ϸ����</returns>
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
    /// ���ݴ���Ĳ������ض������׼���õĶ���
    /// </summary>
    /// <param name="prefab">ָ������Ϸ����Ԥ����</param>
    /// <param name="position">����λ��</param>
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
    /// ���ݴ���Ĳ������ض������׼���õĶ���
    /// </summary>
    /// <param name="prefab">ָ������Ϸ����Ԥ����</param>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">������תֵ</param>
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
    /// ���ݴ���Ĳ������ض������׼���õĶ���
    /// </summary>
    /// <param name="prefab">ָ������Ϸ����Ԥ����</param>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">������תֵ</param>
    /// <param name="localScale">��������ֵ</param>
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
