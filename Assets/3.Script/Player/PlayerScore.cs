using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    private void Start()
    {
        UIManager.instance.addScore(0);
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.CompareTag("Score"))
        {
            UIManager.instance.addScore(1);
        }
    }
}