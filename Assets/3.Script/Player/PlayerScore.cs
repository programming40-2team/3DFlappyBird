using UnityEngine;

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