using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 3;//����������������
    [SerializeField] int scorePoint = 100;//����������������
    [SerializeField] protected int healthFactor;
    LootSpawner lootSpawner; 

    protected virtual void Start()
    {
        lootSpawner = GetComponent<LootSpawner>();
    }
    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }
    public override void Die()
    {
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        ScoreManager.Instance.AddScore(scorePoint);
        EnemyManager.Instance.RemoveFromList(gameObject);
        lootSpawner.Spawn(transform.position);
        base.Die();
    }
    void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber * healthFactor);//���ݲ������ӵ���Ѫ��
    }
}
