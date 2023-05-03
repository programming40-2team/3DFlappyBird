using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Item : MonoBehaviour, IItem
{
    private float highPosition = 10f;
    private float lowPosition = -10f;

    private float moveSpeed = 20;
    private float rotateSpeed = 1000;

    private Vector3 moveDirectionVector = Vector3.up;
    private void OnEnable()
    {
        StartCoroutine(MoveItem_co());
    }
    private IEnumerator MoveItem_co()
    {
        while (true)
        {
            if (transform.position.y >= highPosition || transform.position.y <= lowPosition)
            {
                moveDirectionVector = -moveDirectionVector;
            }
            transform.position += moveSpeed * Time.deltaTime * moveDirectionVector;
            transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public virtual void GetItem(GameObject bird)
    {
        //// UI 매니저에서 점수 추가 
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetItem(other.gameObject);
        }
    }
}