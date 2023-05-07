using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IItem
{
    protected static bool isInvincible;
    private readonly float highPosition = 20f;
    private readonly float lowPosition = 5f;
    private readonly float moveSpeed = 8;
    private readonly float rotateSpeed = 500;

    private readonly int itemScore = 2;

    private Vector3 moveDirectionVector = new Vector3(0, 1, -1);
    private ItemObjectPool itemObjectPool;
    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("ItemPool").TryGetComponent(out itemObjectPool);
        StartCoroutine(MoveItem_co());
    }
    private void Update()
    {
        if (transform.position.y <= -20f)
        {
            itemObjectPool.ReturnObject(gameObject);
        }
    }

    /// <summary>
    /// �������� �ʰ� ���� �ӵ��� �ٰ����鼭 �� �Ʒ��� �̵��ϴ� �ڷ�ƾ
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
    /// �������� �Ծ��� �� ���������� ����� �޼���
    /// </summary>
    /// <param name="bird"></param>
    public virtual void GetItem(GameObject bird)
    {
        UIManager.instance.addScore(itemScore);
        DestroyItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetItem(other.gameObject);
        }
    }

    protected void DestroyItem()
    {
        itemObjectPool.ReturnObject(gameObject);
    }
}