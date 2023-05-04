using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPool : MonoBehaviour
{
    [SerializeField]
    private int count = 15;
    private Queue<GameObject> poolingQueue;
    [SerializeField] private GameObject[] itemArr;

    private void Awake()
    {
        poolingQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            int randNum = Random.Range(0, itemArr.Length);
            GameObject obj = Instantiate(itemArr[randNum], transform.position, itemArr[randNum].transform.rotation, transform);
            poolingQueue.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    public GameObject GetObject()
    {
        if (poolingQueue.Count > 0)
        {
            GameObject obj = poolingQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            int randNum = Random.Range(0, itemArr.Length);
            GameObject obj = Instantiate(itemArr[randNum], transform.position, itemArr[randNum].transform.rotation, transform);
            obj.SetActive(true);
            return obj;
        }
    }
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolingQueue.Enqueue(obj);
    }
}
