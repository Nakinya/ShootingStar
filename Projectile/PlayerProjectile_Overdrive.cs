using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerProjectile_Overdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);//�����������
        this.transform.rotation = Quaternion.identity;//�����ӵ��Ƕ�Ϊ��ʼ
        if (target == null)
        {
            base.OnEnable();//����direction�ƶ�
        }
        else
        {
            StartCoroutine(guidanceSystem.HomingCoroutine(target));//׷��Ŀ��
        }
    }
}
