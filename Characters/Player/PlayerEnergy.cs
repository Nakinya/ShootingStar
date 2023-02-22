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
    int energy;//玩家能量值
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
    public void Obtain(int value)//回复能量值
    {
        if (energy == MAX || !canGetEnergy || !gameObject.activeSelf) return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void Use(int value)//使用能量值
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
    }
    public void overrideUse(int value)//能量爆发时使用能量值
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
        if (energy == 0 && !canGetEnergy)//能量为0且处于能量爆发状态
        {
            PlayerOverdrive.off.Invoke();
        }
    }
    public bool IsEnergyEnough(int value)//判断能量是否足够消耗
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

    IEnumerator KeepUsingCoroutine()//能量持续减少
    {
        while (gameObject.activeSelf && energy > 0)
        {
            overrideUse(PERCENT);
            yield return waitForOverdriveInterval;
        }
    }
}
