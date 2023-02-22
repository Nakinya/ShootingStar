using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class ShieldPickUp : LootItem
{
    [SerializeField] AudioData fullHealthPickUpSFX;
    [SerializeField] int fullHealthBonusScore = 200;//如果血量是满的就增加分数
    [SerializeField] float shielBonus = 20f;//恢复的血量

    protected override void PickUp()
    {
        if (player.IsFullHealth)
        {
            pickUpSFX = fullHealthPickUpSFX;
            lootMessage.text = $"SCORE + {fullHealthBonusScore}";
            ScoreManager.Instance.AddScore(fullHealthBonusScore);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"SHIELD + {shielBonus}";
            player.RestoreHealth(shielBonus);
        }
        base.PickUp();
    }
}
