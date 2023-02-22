using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Character : MonoBehaviour//玩家和敌人继承这个类
{
    [SerializeField] GameObject deathVFX;//死亡视觉特效
    [SerializeField] AudioData[] deathSFX;//死亡爆炸音效
    [Header("-----Health-----")]
    [SerializeField] protected float maxHealth;
    [SerializeField] StatsBar onHeadHealthBar;
    [SerializeField] bool showOnHeadHealthBar = true;
    protected float health;
    bool isDead;//角色死亡状态，防止die()被多次调用

    protected virtual void OnEnable()
    {
        isDead = false;
        health = maxHealth;
        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)//受到伤害
    {
        if (health == 0f) return;
        health -= damage;
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
        if (health <= 0f)
        {
           if (!isDead)
                Die();
           isDead = true;
        }
    }

    public virtual void Die()//死亡时生成一个爆炸特效
    {
        isDead = true;
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        this.gameObject.SetActive(false);//死亡不是摧毁而是禁用
    }

    public virtual void RestoreHealth(float value)//恢复血量
    {
        if(health == maxHealth)
            return;
        health = Mathf.Clamp(health + value, 0f, maxHealth);
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
    }

    protected IEnumerator HeathRegenerateCoroutine(WaitForSeconds waitTime, float percent)//每隔一段时间自动回复百分之的血量
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }protected IEnumerator TakeDamageOvertimeCoroutine(WaitForSeconds waitTime, float percent)//每隔一段时间扣除百分之的血量
    {
        while (health > 0f)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }
}
