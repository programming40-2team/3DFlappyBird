using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    private float width = 20f;
    private Vector3 offset;

    private void Awake()
    {
        offset = new Vector3(0, 0, width * 5);
    }
    void Update()
    {
        if (transform.position.z <= -width)
        {
            Reposition();
        }
    }
    public void Reposition()
    {
        transform.position = transform.position + offset;
    }
}

