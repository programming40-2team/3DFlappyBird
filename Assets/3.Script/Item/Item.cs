using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IItem
{
    private readonly float highPosition = 15f;
    private readonly float lowPosition = 5f;
    private  float moveSpeed = 8;
    private readonly float rotateSpeed = 1000;

    private readonly int itemScore = 2;

    private Vector3 moveDirectionVector = new Vector3(0, 1, -1);

    private void OnEnable()
    {
        moveSpeed = 8;
        StartCoroutine(MoveItem_co());
    }
    /// <summary>
    /// 아이템이 맵과 같은 속도로 다가오면서 위 아래로 이동하는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveItem_co()
    {
        while (true)
        {
            if (transform.position.y >= highPosition || transform.position.y <= lowPosition)
            {
                moveDirectionVector.y = -moveDirectionVector.y;
            }
            transform.position += moveSpeed * Time.deltaTime * moveDirectionVector;
            transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    /// <summary>
    /// 아이템을 먹었을 때 공통적으로 실행될 메서드
    /// </summary>
    /// <param name="bird"></param>
    public virtual void GetItem(GameObject bird)
    {
        UIManager.instance.addScore(itemScore);
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