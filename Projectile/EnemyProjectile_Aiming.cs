using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyProjectile_Aiming : Projectile
{
    private void Awake()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    protected override void OnEnable()
    {
        StartCoroutine(MoveDirectionCoroutine());   
        base.OnEnable();
    }
    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;//在子弹启动的瞬间，由于浮点数的关系子弹的位置可能会存在误差，先等待一帧的时间来让引擎取得精确的数值
        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - this.transform.position).normalized;//子弹方向要归一化，不然子弹速度会很奇怪
        }
    }
}
