using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;//������Ч
    [SerializeField] AudioData[] hitSFX;//������Ч
    [SerializeField] float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;//�ӵ��ƶ�����
    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
    IEnumerator MoveDirectly()//�ӵ��ƶ�
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }
    public void Move()
    {
       transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);//Ĭ��������������ϵ�ƶ����ᵼ���ӵ�����������⣬Ҫ����������ϵ
    }
    public void MoveOnOwn()
    {
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))//��GetComponent�������ܸ����Ҹ��ʺ��ж��Ƿ�ץȡ���ض������
        {
            if (character != null)
            {
                character.TakeDamage(damage);
                var contactPoint = collision.GetContact(0);//��һ���Ӵ������Ϣ
                PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));//����Ӵ����λ��
                AudioManager.Instance.PlayRandomSFX(hitSFX);//���Ż�����Ч
                gameObject.SetActive(false);
            }
        }
    }

    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
