using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;
    public void Spawn(Vector2 position)//生成掉落物
    {
        foreach (var item in lootSettings)
        {
            item.Spawn(position + Random.insideUnitCircle);//让生成位置有一点随机偏移
        }
    }
}
