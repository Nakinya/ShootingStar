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
        if (collision.gameObject.TryGetComponent<Player>(out Player player))//��GetComponent�������ܸ����Ҹ��ʺ��ж��Ƿ�ץȡ���ض������
        {
            player.TakeDamage(damage);
            var contactPoint = collision.GetContact(0);//��һ���Ӵ������Ϣ
            PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));//����Ӵ����λ��
            
        }
    }
}
