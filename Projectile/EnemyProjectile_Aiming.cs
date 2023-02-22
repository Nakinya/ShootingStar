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
        yield return null;//���ӵ�������˲�䣬���ڸ������Ĺ�ϵ�ӵ���λ�ÿ��ܻ�������ȵȴ�һ֡��ʱ����������ȡ�þ�ȷ����ֵ
        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - this.transform.position).normalized;//�ӵ�����Ҫ��һ������Ȼ�ӵ��ٶȻ�����
        }
    }
}
