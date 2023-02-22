using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyBeam : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject hitVFX;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))//比GetComponent消耗性能更少且更适合判断是否抓取到特定的组件
        {
            player.TakeDamage(damage);
            var contactPoint = collision.GetContact(0);//第一个接触点的信息
            PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));//传入接触点的位置
            
        }
    }
}
