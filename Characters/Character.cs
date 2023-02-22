using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Character : MonoBehaviour//��Һ͵��˼̳������
{
    [SerializeField] GameObject deathVFX;//�����Ӿ���Ч
    [SerializeField] AudioData[] deathSFX;//������ը��Ч
    [Header("-----Health-----")]
    [SerializeField] protected float maxHealth;
    [SerializeField] StatsBar onHeadHealthBar;
    [SerializeField] bool showOnHeadHealthBar = true;
    protected float health;
    bool isDead;//��ɫ����״̬����ֹdie()����ε���

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

    public virtual void TakeDamage(float damage)//�ܵ��˺�
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

    public virtual void Die()//����ʱ����һ����ը��Ч
    {
        isDead = true;
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        this.gameObject.SetActive(false);//�������Ǵݻٶ��ǽ���
    }

    public virtual void RestoreHealth(float value)//�ָ�Ѫ��
    {
        if(health == maxHealth)
            return;
        health = Mathf.Clamp(health + value, 0f, maxHealth);
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
    }

    protected IEnumerator HeathRegenerateCoroutine(WaitForSeconds waitTime, float percent)//ÿ��һ��ʱ���Զ��ظ��ٷ�֮��Ѫ��
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }protected IEnumerator TakeDamageOvertimeCoroutine(WaitForSeconds waitTime, float percent)//ÿ��һ��ʱ��۳��ٷ�֮��Ѫ��
    {
        while (health > 0f)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }
}
