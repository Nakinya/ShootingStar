using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class MisslePickUp : LootItem
{
    protected override void PickUp()
    {
        player.PickUpMissile();
        base.PickUp();
    }
}
