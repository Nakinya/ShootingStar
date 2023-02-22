using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;
    public void Spawn(Vector2 position)//���ɵ�����
    {
        foreach (var item in lootSettings)
        {
            item.Spawn(position + Random.insideUnitCircle);//������λ����һ�����ƫ��
        }
    }
}
