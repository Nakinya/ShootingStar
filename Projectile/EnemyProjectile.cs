using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyProjectile : Projectile
{
    private void Awake()
    {
        if (moveDirection != Vector2.left)//��ת�ӵ�������ƶ�����һ��
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
        }
    }
}
