using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : Item
{
    public override void GetItem(GameObject bird)
    {

        if (!bird.GetComponent<PlayerControl>().isInvincible)
        {
            bird.GetComponent<PlayerControl>().DeathCount++;
            UIManager.instance.isPlayerLifeIncrease(true);
            base.GetItem(bird);
        }
        
    }
}