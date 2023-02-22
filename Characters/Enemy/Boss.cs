using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Boss : Enemy
{
    BossHealthBar healthBar;
    Canvas bossHealthBarCanvas;
    Canvas healthBarCanvas;
    private void Awake()
    {
        healthBar = FindObjectOfType<BossHealthBar>();
        bossHealthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
        healthBarCanvas = GetComponentInChildren<Canvas>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health,maxHealth);
        bossHealthBarCanvas.enabled = true;
        healthBarCanvas.enabled= true;
    }
    private void OnDisable()
    {
        bossHealthBarCanvas.enabled = false;
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
        }
    }
    public override void Die()
    {
        bossHealthBarCanvas.enabled = false;
        healthBarCanvas.enabled = false;
        base.Die();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateStats(health, maxHealth);
    }
}
