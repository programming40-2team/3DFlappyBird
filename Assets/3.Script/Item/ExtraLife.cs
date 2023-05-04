using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : Item
{
    public override void GetItem(GameObject bird)
    {
        bird.GetComponent<PlayerControl>().deathCount++;
        base.GetItem(bird);
    }
}