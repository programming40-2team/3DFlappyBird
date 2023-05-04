using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    private int playerScore;

    [SerializeField] private Text ScoreText;

    void Awake()
    {
        playerScore = 0;
    }


    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.CompareTag("Score"))
        {
            playerScore++;
        }
    }
}