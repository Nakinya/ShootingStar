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
        SetTarget(EnemyManager.Instance.RandomEnemy);//设置随机敌人
        this.transform.rotation = Quaternion.identity;//重置子弹角度为初始
        if (target == null)
        {
            base.OnEnable();//朝着direction移动
        }
        else
        {
            StartCoroutine(guidanceSystem.HomingCoroutine(target));//追踪目标
        }
    }
}
