using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenningAnimation : MonoBehaviour
{

    [SerializeField]
    private GameObject titleBroken;
    private void Start()
    {
        titleBroken.gameObject.SetActive(false);

    }
    public void breakGlass()
    {
        titleBroken.SetActive(true);
        StartCoroutine(nameof(fadeEffect), titleBroken.transform.parent);

    }

    private IEnumerator fadeEffect(GameObject go)
    {
        float fadeSpeed = 0.5f;

        Image fadeImage = go.GetComponent<Image>();
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
}
