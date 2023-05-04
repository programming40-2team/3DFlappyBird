using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float moveSpeed;

    private void Update()
    {
        gameObject.transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
