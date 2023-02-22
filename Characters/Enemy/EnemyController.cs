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
    protected float paddingX;//��ɫ���ұ߾�
    protected float paddingY;//��ɫ���±߾�
    [SerializeField] float moveRotationAngle = 25f;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    protected Vector3 targetPosition;

    [Header("-----Fire-----")]
    [SerializeField] protected GameObject[] projectiles;//�ӵ�Ԥ����
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
    IEnumerator RandomlyMovingCoroutine()//����ƶ�
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX,paddingY);
        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY); 
        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime)//һ���ӽ���0����,�ж��Ƿ񵽴�Ŀ��λ��
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);//�����time.deltaTime����֡��̫��ʱ���ܵ��ﲻ��Ŀ���
                //�ƶ�ʱ��X����ת
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }
            //yield return null;
            yield return waitForFixedUpdate;//����ֱ����һ���̶�֡
        }
    }
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));//������ʱ��

            if (GameManager.GameState == GameState.GameOver) yield break;
           
            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(fireSFX);
            muzzleVFX.Play();//�������ѭ�����ŵ�
        }
    }
}
