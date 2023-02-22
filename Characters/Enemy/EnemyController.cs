using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyController : MonoBehaviour
{
    [Header("-----Move-----")]
    [SerializeField] float moveSpeed = 2f;
    protected float paddingX;//角色左右边距
    protected float paddingY;//角色上下边距
    [SerializeField] float moveRotationAngle = 25f;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    protected Vector3 targetPosition;

    [Header("-----Fire-----")]
    [SerializeField] protected GameObject[] projectiles;//子弹预制体
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected AudioData[] fireSFX;
    [SerializeField] protected ParticleSystem muzzleVFX;

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }
    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator RandomlyMovingCoroutine()//随机移动
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX,paddingY);
        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY); 
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime)//一个接近于0的数,判断是否到达目标位置
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);//如果是time.deltaTime，当帧数太低时可能到达不了目标点
                //移动时绕X轴旋转
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }
            //yield return null;
            yield return waitForFixedUpdate;//挂起直到下一个固定帧
        }
    }
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));//随机间隔时间

            if (GameManager.GameState == GameState.GameOver) yield break;
           
            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(fireSFX);
            muzzleVFX.Play();//这个不是循环播放的
        }
    }
}
