using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;//击中特效
    [SerializeField] AudioData[] hitSFX;//击中音效
    [SerializeField] float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;//子弹移动方向
    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
    IEnumerator MoveDirectly()//子弹移动
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }
    public void Move()
    {
       transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);//默认是沿自身坐标系移动，会导致子弹朝向出现问题，要沿世界坐标系
    }
    public void MoveOnOwn()
    {
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))//比GetComponent消耗性能更少且更适合判断是否抓取到特定的组件
        {
            if (character != null)
            {
                character.TakeDamage(damage);
                var contactPoint = collision.GetContact(0);//第一个接触点的信息
                PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));//传入接触点的位置
                AudioManager.Instance.PlayRandomSFX(hitSFX);//播放击中音效
                gameObject.SetActive(false);
            }
        }
    }

    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
