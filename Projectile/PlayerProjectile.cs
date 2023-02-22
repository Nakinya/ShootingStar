using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerProjectile : Projectile
{
    TrailRenderer trail;
    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        if (moveDirection != Vector2.right)//��ת�ӵ�������ƶ�����һ��
        {
            //transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
            transform.rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);//��projectile�ﲻ�޸������������ƶ�������ת�����壬��Ȼ��ת�����������
        }
    }
    private void OnDisable()
    {
        if(trail == null)
            trail = GetComponentInChildren<TrailRenderer>();
        trail.Clear();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)//����ӵ����к�������
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
    }
}
