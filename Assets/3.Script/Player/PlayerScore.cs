using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private void Start()
    {
        UIManager.instance.addScore(0);
    }

    private void OnTriggerExit(Collider coll)
    {
        Debug.Log("실행0");
        Debug.Log(coll.transform.tag);
        if (coll.CompareTag("Score"))
        {
            
            Debug.Log("실행1");
            UIManager.instance.addScore(1);
        }
    }
}