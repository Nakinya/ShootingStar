using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class ShieldPickUp : LootItem
{
    [SerializeField] AudioData fullHealthPickUpSFX;
    [SerializeField] int fullHealthBonusScore = 200;//���Ѫ�������ľ����ӷ���
    [SerializeField] float shielBonus = 20f;//�ָ���Ѫ��

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
