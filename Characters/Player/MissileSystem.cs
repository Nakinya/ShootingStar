using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;//默认导弹数量
    [SerializeField] float cooldownTime = 1f;//导弹冷却时间
    [SerializeField] GameObject missilePrefab;
    [SerializeField] AudioData launchSFX;
    int amount;
    bool isReady = true;//导弹是否准备好
    private void Awake()
    {
        amount = defaultAmount;
    }
    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }
    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady) return;
        isReady = false;
        PoolManager.Release(missilePrefab,muzzleTransform.position);
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        amount--;
        MissileDisplay.UpdateAmountText(amount);
        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);//导弹为0时更新UI图片
        }
        else
        {
            StartCoroutine(CoolDownCoroutine());
        }
    }
    IEnumerator CoolDownCoroutine()
    {
        float cooldownValue = cooldownTime;
        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);//更新UI图片
            cooldownValue = Mathf.Max(0, cooldownValue - Time.deltaTime);
            
            yield return null;
        }
        isReady = true;
    }
    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 1)//注意更新导弹图标
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }
}
