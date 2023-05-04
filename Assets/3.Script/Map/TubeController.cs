using UnityEngine;

public class TubeController : MonoBehaviour
{
    private ObjectPool objectPool;
    private float width = 20f;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("TubePool").TryGetComponent(out objectPool);
    }
    void Update()
    {
        if (transform.position.z <= -width)
        {
            objectPool.ReturnObject(gameObject);
        }
    }
}
