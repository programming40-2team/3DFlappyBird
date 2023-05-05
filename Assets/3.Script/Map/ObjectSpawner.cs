using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private ObjectPool tubeObjectPool;
    private ItemObjectPool itemObjectPool;
    private float randomNum;
    [SerializeField] private float spawnTime;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("TubePool").TryGetComponent(out tubeObjectPool);
        GameObject.FindGameObjectWithTag("ItemPool").TryGetComponent(out itemObjectPool);
        StartCoroutine(SpawnTube_co());
    }

    private void SetRandomPosition()
    {
        randomNum = Random.Range(9f, 20f);
    }

    private IEnumerator SpawnTube_co()
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnTime / 2);
        
        while (true)
        {
            SetRandomPosition();
            //GameObject obj = tubeObjectPool.GetObject();
            //obj.transform.position = new Vector3(transform.position.x, randomNum, transform.position.z);
            tubeObjectPool.GetObject().transform.position = new Vector3(transform.position.x, randomNum, transform.position.z);
            yield return wfs;
            if (IsSuccessSummonItem())
            {
                itemObjectPool.GetObject().transform.position = transform.position + Vector3.up * 10;
            }
            yield return wfs;
        }
    }

    private bool IsSuccessSummonItem()
    {
        int RandNumber = Random.Range(0, 100);
        if(RandNumber >= 85)
        {
            return true;
        }
        return false;
    }
}
