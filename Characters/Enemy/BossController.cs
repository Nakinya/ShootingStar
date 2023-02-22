using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class BossController : EnemyController
{
    [SerializeField] float continousFireDuration = 1.5f;//�������ʱ��
    [Header("-----Player Detection-----")]
    [SerializeField] Transform playerDetectionTransform;
    [SerializeField] Vector3 playerDetectionSize;//���гߴ�
    [SerializeField] LayerMask playerLayer;//��Ҳ�����

    [Header("------Beam-----")]
    [SerializeField] float beamCoolDown = 12f;
    [SerializeField] AudioData beamChargingSFX;
    [SerializeField] AudioData beamLaunchSFX;
    bool isBeamReady;
    int launchBeamID = Animator.StringToHash("launchBeam");
    Animator animator;
    WaitForSeconds waitForBeamCoolDownTime;

    WaitForSeconds waitForContinousFireInterval;//�����ӵ����ʱ��
    WaitForSeconds waitForFireInterval;//ÿ�ֹ������ʱ��

    List<GameObject> magazine;//��ϻ
    AudioData launchSFX; //������Ч

    Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>(); 
        waitForContinousFireInterval = new WaitForSeconds(minFireInterval);
        waitForFireInterval = new WaitForSeconds(maxFireInterval);
        waitForBeamCoolDownTime = new WaitForSeconds(beamCoolDown);

        magazine = new List<GameObject>(projectiles.Length);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void OnEnable()
    {
        isBeamReady = false;
        muzzleVFX.Stop();
        StartCoroutine(BeamCoolDownCoroutine());
        base.OnEnable();
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(playerDetectionTransform.position, playerDetectionSize);
    //}
    void LoadProjectiles()
    {
        magazine.Clear();
        if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))//�����boss��ǰ����װ��1���ӵ�,�ӵ�Ԥ����������inspector�鿴
        {
            magazine.Add(projectiles[0]);
            launchSFX = fireSFX[0];
        }
        else//װ��2�Ż�3���ӵ�
        {
            if (Random.value < 0.5)
            {
                magazine.Add(projectiles[1]);
                launchSFX = fireSFX[1];
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }
                launchSFX = fireSFX[2];
            }
        }
    }
    protected override IEnumerator RandomlyFireCoroutine()
    {
        while (isActiveAndEnabled)
        {
            yield return waitForFireInterval;//��������Э��֮��ļ��
            if (GameManager.GameState == GameState.GameOver) yield break;

            if (isBeamReady)
            {
                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));//��������ʱ׷�����
                yield break;
            }
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }
    IEnumerator ContinuousFireCoroutine()//boss��������Э��
    {
        LoadProjectiles();//װ���ӵ�
        muzzleVFX.Play();
        float continuousFireTimer = 0f;//��ʱ��
        while (continuousFireTimer < continousFireDuration)
        {
            foreach (var projectile in magazine)//���䵯ϻ�е��ӵ�
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);//���Ŷ�Ӧ��Ч

            yield return waitForContinousFireInterval;
        }
        muzzleVFX.Stop();
    }
    void ActivateBeamWeapon()
    {
        isBeamReady = false;
        AudioManager.Instance.PlaySFX(beamChargingSFX);
        animator.SetTrigger(launchBeamID);
    }
    void AnimationEventLaunchBeam()//�����¼�
    {
        AudioManager.Instance.PlaySFX(beamLaunchSFX);
    }
    void AnimationEventStopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        StartCoroutine(BeamCoolDownCoroutine());
        StartCoroutine(RandomlyFireCoroutine());
    }

    IEnumerator BeamCoolDownCoroutine()
    {
        yield return waitForBeamCoolDownTime;
        isBeamReady = true;
    }
    IEnumerator ChasingPlayerCoroutine()//׷�����
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            targetPosition.y = playerTransform.position.y;

            yield return null;  
        }
    }
}
