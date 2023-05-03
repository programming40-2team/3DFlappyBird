using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    private float width = 20f;
    
    void Update()
    {
        if (transform.position.z <= -width)
        {
            Reposition();
        }
    }
    public void Reposition()
    {
        Vector3 offset = new Vector3(0, 0, width * 5);
        transform.position = (Vector3)transform.position + offset;
    }
}

