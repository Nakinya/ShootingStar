using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 3;//敌人死亡能量奖励
    [SerializeField] int scorePoint = 100;//敌人死亡分数奖励
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
        maxHealth += (int)(EnemyManager.Instance.WaveNumber * healthFactor);//根据波数增加敌人血量
    }
}
