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
        if (moveDirection != Vector2.right)//旋转子弹方向和移动方向一致
        {
            //transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
            transform.rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);//若projectile里不修改沿世界坐标移动，则旋转子物体，不然旋转方向会有问题
        }
    }
    private void OnDisable()
    {
        if(trail == null)
            trail = GetComponentInChildren<TrailRenderer>();
        trail.Clear();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)//玩家子弹击中后获得能量
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
    }
}
