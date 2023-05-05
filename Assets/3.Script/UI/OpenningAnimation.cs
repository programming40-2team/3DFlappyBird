using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenningAnimation : MonoBehaviour
{

    [SerializeField]
    private GameObject titleBroken;

    [SerializeField]
    private GameObject ShowMap;

    private void Start()
    {
        titleBroken.gameObject.SetActive(false);

    }
    public void breakGlass()
    {
        titleBroken.SetActive(true);
        StartCoroutine(nameof(fadeEffect));

    }

    private IEnumerator fadeEffect()
    {
        float fadeSpeed = 0.5f;

        Image fadeImage = titleBroken.transform.parent.GetComponent<Image>();
        float alpha = fadeImage.color.a;
        if (alpha == 1)
        {
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
        else
        {
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fadeSpeed;
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
    }

    public void GlassDisappearAndShowMap()
    {
        titleBroken.gameObject.SetActive(false);
        ShowMap.gameObject.SetActive(true);

    }

}
