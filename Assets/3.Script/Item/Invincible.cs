using System.Collections;
using UnityEngine;

public class Invincible : Item
{
    [SerializeField] private Shader shader;
    [SerializeField] private Shader defaultShader;

    private readonly float invincibleTime = 20f;
    public override void GetItem(GameObject bird)
    {
        if (!bird.GetComponent<PlayerControl>().isInvincible && !isInvincible)
        {
            StartCoroutine(InvincibleItem_co(bird));
            UIManager.instance.addScore(2);
            GetComponent<Renderer>().enabled = false;
        }
    }

    private IEnumerator InvincibleItem_co(GameObject bird)
    {
        Collider birdColl = bird.GetComponent<Collider>();
        isInvincible = true;
        ChangeShader(bird, shader);
        birdColl.isTrigger = true;
        Time.timeScale = 2f;
        SoundManager.Instance.PlaySuperStar();
        yield return new WaitForSeconds(invincibleTime);
        ChangeShader(bird, defaultShader);
        birdColl.isTrigger = false;
        Time.timeScale = 1f;
        SoundManager.Instance.StopSuperStar();
        isInvincible = false;
        DestroyItem();
    }
    private void ChangeShader(GameObject bird, Shader shader_)
    {
        foreach (Transform child in bird.transform)
        {
            if (child.TryGetComponent(out Renderer ren))
            {
                for (int i = 0; i < ren.materials.Length; i++)
                {
                    ren.materials[i].shader = shader_;
                }
            }
        }
    }
}
