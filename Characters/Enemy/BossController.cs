using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class BossController : EnemyController
{
    [SerializeField] float continousFireDuration = 1.5f;//开火持续时间
    [Header("-----Player Detection-----")]
    [SerializeField] Transform playerDetectionTransform;
    [SerializeField] Vector3 playerDetectionSize;//检测盒尺寸
    [SerializeField] LayerMask playerLayer;//玩家层遮罩

    [Header("------Beam-----")]
    [SerializeField] float beamCoolDown = 12f;
    [SerializeField] AudioData beamChargingSFX;
    [SerializeField] AudioData beamLaunchSFX;
    bool isBeamReady;
    int launchBeamID = Animator.StringToHash("launchBeam");
    Animator animator;
    WaitForSeconds waitForBeamCoolDownTime;

    WaitForSeconds waitForContinousFireInterval;//发射子弹间隔时间
    WaitForSeconds waitForFireInterval;//每轮攻击间隔时间

    List<GameObject> magazine;//弹匣
    AudioData launchSFX; //发射音效

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
        if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))//玩家在boss正前方则装填1号子弹,子弹预制体数组在inspector查看
        {
            magazine.Add(projectiles[0]);
            launchSFX = fireSFX[0];
        }
        else//装填2号或3号子弹
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
            yield return waitForFireInterval;//连续开火协程之间的间隔
            if (GameManager.GameState == GameState.GameOver) yield break;

            if (isBeamReady)
            {
                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));//开启激光时追击玩家
                yield break;
            }
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }
    IEnumerator ContinuousFireCoroutine()//boss连续开火协程
    {
        LoadProjectiles();//装填子弹
        muzzleVFX.Play();
        float continuousFireTimer = 0f;//计时器
        while (continuousFireTimer < continousFireDuration)
        {
            foreach (var projectile in magazine)//发射弹匣中的子弹
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);//播放对应音效

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
    void AnimationEventLaunchBeam()//动画事件
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
    IEnumerator ChasingPlayerCoroutine()//追踪玩家
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            targetPosition.y = playerTransform.position.y;

            yield return null;  
        }
    }
}
