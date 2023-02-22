using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;
    [SerializeField] bool canRegenerateHealth = true;
    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;
    readonly float InvincibleTime = 1f;//受击无敌时间

    [Header("-----Input-----")]
    [SerializeField] PlayerInput input;

    [Header("-----Move-----")]
    [SerializeField]float moveSpeed = 10f;
     float paddingX = 0.2f;//角色左右边距
     float paddingY = 0.2f;//角色上下边距
    [SerializeField] float accelerationTime = 3f;//加速时间
    [SerializeField] float decelerationTime = 3f;//减速时间
    [SerializeField] float moveRotationAngle = 50f;//角色上下移动翻滚角度
    float t;//移动进行时间
    Vector2 moveDirection;//解决玩家被子弹撞击后会停止移动的问题
    Vector2 previousVelocity;//存储移动前的速度
    Quaternion previousRotation;//存储移动前的旋转值

    [Header("-----Fire-----")]
    [SerializeField] GameObject projectile1;//子弹种类
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] GameObject projectileOverdrive;
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    [SerializeField] Transform muzzleMiddle;//子弹发射位置
    [SerializeField] Transform muzzleTop;//子弹发射位置
    [SerializeField] Transform muzzleButtom;//子弹发射位置
    [SerializeField] float fireInterval = 0.2f;
    [SerializeField] AudioData fireSFX;
    [SerializeField] ParticleSystem muzzleVFX;

    [Header("-----Dodge-----")]
    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;
    [SerializeField] float maxRoll = 720f;//翻滚角度
    [SerializeField] float rollSpeed = 360f;//翻滚时间
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//翻滚缩放值
    [SerializeField] AudioData dodgeSFX;
    float currentRoll;
    bool isDodging = false;
    float dodgeDuration;//翻滚进行的时间

    [Header("-----Overdrive-----")]
    bool isOverdriving = false;//是否处于能量爆发状态
    [SerializeField] int OverdriveDodgeFactor = 2;//能量爆发状态时能力值的变化
    [SerializeField] float OverdriveSpeedFactor = 1.2f;
    [SerializeField] float OverDriveFireFactor = 1.2f;
    readonly float SlowMotionDuration = 0.5f;

    public bool IsFullHealth => health == maxHealth;
    public bool IsFullPower => weaponPower == 2;


    MissileSystem missile;

    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForHealthGenerateTime;
    WaitForSeconds waitForOverdriveFireInterval;//能量爆发时的开火间隔
    WaitForSeconds waitForInvincibleTime;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    new Rigidbody2D rigidbody;
    new Collider2D collider;
    private void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        missile = GetComponent<MissileSystem>();
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForHealthGenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / OverDriveFireFactor);
        waitForInvincibleTime = new WaitForSeconds(InvincibleTime);
        dodgeDuration = maxRoll / rollSpeed;
    }
    
    private void Start()
    {
        statsBar_HUD.Initialize(health, maxHealth);
        rigidbody.gravityScale = 0f;
        input.EnableGameplayInput();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }
    #region Health
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PowerDown();
        statsBar_HUD.UpdateStats(health, maxHealth);
        if (gameObject.activeSelf)
        {
            Move(moveDirection);//解决玩家速度会被子弹抵消的问题
            StartCoroutine(InvincibleCoroutine());//玩家受伤进入无敌状态
            if (canRegenerateHealth)
            {
                if (healthRegenerateCoroutine != null)//删除重复协程
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                healthRegenerateCoroutine = StartCoroutine(HeathRegenerateCoroutine(waitForHealthGenerateTime, healthRegeneratePercent));
            }
        }
    }
    IEnumerator InvincibleCoroutine()
    {
        collider.isTrigger = true;
        yield return waitForInvincibleTime;
        collider.isTrigger = false;
    }
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }
    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        Debug.Log("PlayerDie");
        base.Die();
    }

    #endregion
    #region Move
    private void Move(Vector2 moveInput)
    {
        // Vector2 moveAmount = moveInput * moveSpeed;
        //rigidbody.velocity = moveInput * moveSpeed;
        if (moveCoroutine != null)//在开始新的协程前把上一次协程关闭
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = moveInput.normalized;
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection*moveSpeed,moveRotation));
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    
    private void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = Vector2.zero;//避免玩家不动时受伤出现不受控制的移动
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, moveDirection, Quaternion.identity));

        if (rigidbody.velocity == Vector2.zero)
        {
            StopCoroutine(nameof(MoveRangeLimitationCoroutine));
        }
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)//优化玩家移动
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;
        //while (t < 1f)
        //{
        //    t += Time.fixedDeltaTime / time;
        //    rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);
        //    transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);
        //    yield return new WaitForFixedUpdate;
        //}
        while (t < time)
        {
            t += Time.fixedDeltaTime;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);
            yield return null;
        }
    }
  
    IEnumerator MoveRangeLimitationCoroutine()//在协程里去检测玩家移动范围而不是update，节约性能
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(this.transform.position,paddingX,paddingY);
            yield return null;
        }
    }
    #endregion Move
    #region Fire
    private void Fire()
    {
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }
    private void StopFire()
    {
        muzzleVFX.Stop();
        //StopCoroutine(FireCoroutine());//unity的老问题，直接传入协程StopCoroutine不会正常工作，用第二个重载
        StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            AudioManager.Instance.PlayRandomSFX(fireSFX);//播放开火音效
            switch (weaponPower)//根据威力切换子弹
            {
                //case 0:
                //    Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
                //    break;
                //case 1:
                //    Instantiate(projectile1, muzzleTop.position, Quaternion.identity);
                //    Instantiate(projectile1, muzzleButtom.position, Quaternion.identity);
                //    break;
                //case 2:
                //    Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
                //    Instantiate(projectile2, muzzleTop.position, Quaternion.identity);
                //    Instantiate(projectile3, muzzleButtom.position, Quaternion.identity);
                //    break;
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleButtom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleButtom.position);
                    break;
            }
            //yield return waitForFireInterval;
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    #endregion Fire
    #region Dodge
    private void Dodge()//翻滚
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnergyEnough(dodgeEnergyCost)) 
            return;
        StartCoroutine(DodgeCoroutine());
        
    }
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlaySFX(dodgeSFX);//播放闪避音效
        //消耗能量
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //让玩家无敌
        collider.isTrigger = true;
        //让玩家绕X轴旋转
        //改变玩家缩放值模拟向纵深方向移动闪避
        currentRoll = 0f;//开始旋转前将滚转角归零
        //*Method 1
        //Vector3 scale = transform.localScale;
        //while (currentRoll < maxRoll)
        //{
        //    currentRoll += rollSpeed *Time.deltaTime;
        //    transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);//旋转
        //    //需要将缩放值限制在最小缩放值和1之间
        //    if (currentRoll < maxRoll / 2f)
        //    {
        //        scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f); 
        //        scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f); 
        //        scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f); 
        //    }
        //    else
        //    {
        //        scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //        scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //        scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //    }
        //    transform.localScale = scale;

        //    yield return null;
        //}

        //*Method 2
        //float t1 = 0f;
        //float t2 = 0f;
        //while (currentRoll < maxRoll)
        //{
        //    currentRoll += rollSpeed * Time.deltaTime;
        //    transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
        //    if (currentRoll < maxRoll / 2f)
        //    {
        //        t1 += Time.deltaTime / dodgeDuration;
        //        transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);
        //    }
        //    else
        //    {
        //        t2 += Time.deltaTime / dodgeDuration;
        //        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
        //    }
        //    yield return null;
        //}

        //*Method3
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);
            yield return null;
        }
        collider.isTrigger = false;
        isDodging=false;
    }

    Vector3 QuadraticPoint(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float by)//二次贝塞尔曲线
    {
        return Vector3.Lerp(
            Vector3.Lerp(startPoint,controlPoint,by),
            Vector3.Lerp(controlPoint,endPoint,by),
            by
            );
    }
    #endregion
    #region Overdrive
    private void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnergyEnough(PlayerEnergy.MAX)) //能量是否全满
            return;
        PlayerOverdrive.on.Invoke();
    }
    void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= OverdriveDodgeFactor;//削弱防御能力
        moveSpeed *= OverdriveSpeedFactor;
        TimeController.Instance.BulletTime(SlowMotionDuration, SlowMotionDuration);
    }
    void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= OverdriveDodgeFactor;//削弱防御能力
        moveSpeed /= OverdriveSpeedFactor;
    }
    #endregion
    #region Missile
    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
    public void PickUpMissile()
    {
        missile.PickUp();
    }
    #endregion
    #region Weapon Power
    public void PowerUp()
    {
        weaponPower = Mathf.Min(++weaponPower, 2);
    }
    void PowerDown()
    {
        weaponPower = Mathf.Max(--weaponPower, 0);
    }
    #endregion
}
