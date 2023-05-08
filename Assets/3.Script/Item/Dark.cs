using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Dark : Item
{
    private float darkTime = 5f;
  
    public override void GetItem(GameObject bird)
    {
        if (!bird.GetComponent<PlayerControl>().isInvincible && !isInvincible)
        {
            StartCoroutine(ToDark());
            GetComponent<Renderer>().enabled = false;
        }
    }

    private IEnumerator ToDark()
    {
        Camera.main.GetComponent<Volume>().enabled = true;
        yield return new WaitForSeconds(darkTime);
        Camera.main.GetComponent<Volume>().enabled = false;
        DestroyItem();
    }
}
