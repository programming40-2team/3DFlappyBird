using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private int count;
    private Queue<GameObject> poolingQueue;
    [SerializeField] private GameObject[] tubeArr = new GameObject[3];

    private void Awake()
    {
        poolingQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            int randNum = Random.Range(0, tubeArr.Length);
            GameObject obj = Instantiate(tubeArr[randNum], transform.position, tubeArr[randNum].transform.rotation, transform);
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
            int randNum = Random.Range(0, tubeArr.Length);
            GameObject obj = Instantiate(tubeArr[randNum], transform.position, tubeArr[randNum].transform.rotation, transform);
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
