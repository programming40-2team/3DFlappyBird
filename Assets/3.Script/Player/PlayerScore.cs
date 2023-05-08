using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Score"))
        {
            UIManager.instance.addScore(1);
        }
    }
}