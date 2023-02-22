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
        if (moveDirection != Vector2.left)//旋转子弹方向和移动方向一致
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
        }
    }
}
