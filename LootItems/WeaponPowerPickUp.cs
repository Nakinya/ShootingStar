using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class WeaponPowerPickUp : LootItem
{
    [SerializeField] AudioData fullPowerPickUpSFX;
    [SerializeField] int fullPowerBonusScore = 200;//如果威力是满的就增加分数

    protected override void PickUp()
    {
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"SCORE + {fullPowerBonusScore}";
            ScoreManager.Instance.AddScore(fullPowerBonusScore);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"POWER UP!";
            player.PowerUp();
        }
        base.PickUp();
    }
}
