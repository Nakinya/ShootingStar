using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerMissile : PlayerProjectile_Overdrive
{
    [SerializeField] AudioData targetAquiredVoice = null;
    [Header("-----SpeedChange-----")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 25f;
    [SerializeField] float variableSpeedDelay = 0.5f; //变速延迟时间

    [Header("-----Explosion-----")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioData explosionSFX;
    [SerializeField] LayerMask enemyLayerMask = default;//层级遮罩
    [SerializeField] float explosionRange = 3f;
    [SerializeField] float explosionAOEDamage = 30f;

    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(VariableSpeedCoroutine());
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PoolManager.Release(explosionVFX, transform.position);
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        //导弹爆炸范围伤害 1、触发器检测  2、距离检测 3、物理重叠函数
        //物理重叠函数
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRange, enemyLayerMask);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionAOEDamage);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;//线条颜色为黄色
        Gizmos.DrawWireSphere(transform.position,explosionRange);//画出球体
    }
#endif

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;
        yield return waitVariableSpeedDelay;
        moveSpeed = highSpeed;
        if (target != null)
        {
            AudioManager.Instance.PlaySFX(targetAquiredVoice);
        }
    }

}
