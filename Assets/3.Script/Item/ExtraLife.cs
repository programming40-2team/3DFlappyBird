using UnityEngine;

public class ExtraLife : Item
{
    public override void GetItem(GameObject bird)
    {
        if (!bird.GetComponent<PlayerControl>().isInvincible && !isInvincible)
        {
            bird.GetComponent<PlayerControl>().DeathCount++;
            UIManager.instance.isPlayerLifeIncrease(true);
            base.GetItem(bird);
        }
    }
}