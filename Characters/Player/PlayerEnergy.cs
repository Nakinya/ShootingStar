using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PlayerEnergy : Singleton<PlayerEnergy>
{
    public const int MAX = 100;
    public const int PERCENT = 1;
    int energy;//�������ֵ
    bool canGetEnergy = true;
    [SerializeField] EnergyBar energyBar;
    [SerializeField] float overdriveInterval = 0.1f;

    WaitForSeconds waitForOverdriveInterval;

    protected override  void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    private void Start()
    {
        energyBar.Initialize(energy,MAX);
        Obtain(MAX);
    }
    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }
    public void Obtain(int value)//�ظ�����ֵ
    {
        if (energy == MAX || !canGetEnergy || !gameObject.activeSelf) return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void Use(int value)//ʹ������ֵ
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
    }
    public void overrideUse(int value)//��������ʱʹ������ֵ
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
        if (energy == 0 && !canGetEnergy)//����Ϊ0�Ҵ�����������״̬
        {
            PlayerOverdrive.off.Invoke();
        }
    }
    public bool IsEnergyEnough(int value)//�ж������Ƿ��㹻����
    {
        return energy >= value;
    }
    void PlayerOverdriveOn()
    {
        canGetEnergy = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    void PlayerOverdriveOff()
    {
        canGetEnergy = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()//������������
    {
        while (gameObject.activeSelf && energy > 0)
        {
            overrideUse(PERCENT);
            yield return waitForOverdriveInterval;
        }
    }
}
