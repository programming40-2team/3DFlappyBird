using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private int count = 15;
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
        //���ھ� �ݶ��̴� �±װ� TubePool�� ���ϴ� ���� ���� �ӽ� ��ġ
        foreach (Transform child in transform)
        {
            for(int i = 0; i < child.childCount; i++)
            {
                string obTag;
                if(i == 2)
                {
                    obTag = "Score";
                }
                else
                {
                    obTag = "Tube";
                }
                child.GetChild(i).tag = obTag;
            }
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
