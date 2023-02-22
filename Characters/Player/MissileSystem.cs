using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;//Ĭ�ϵ�������
    [SerializeField] float cooldownTime = 1f;//������ȴʱ��
    [SerializeField] GameObject missilePrefab;
    [SerializeField] AudioData launchSFX;
    int amount;
    bool isReady = true;//�����Ƿ�׼����
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
            MissileDisplay.UpdateCooldownImage(1f);//����Ϊ0ʱ����UIͼƬ
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
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);//����UIͼƬ
            cooldownValue = Mathf.Max(0, cooldownValue - Time.deltaTime);
            
            yield return null;
        }
        isReady = true;
    }
    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 1)//ע����µ���ͼ��
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }
}
