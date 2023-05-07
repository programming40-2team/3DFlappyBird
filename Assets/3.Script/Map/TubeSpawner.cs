using System.Collections;
using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
    private ObjectPool objectPool;
    private float randomNum;
    [SerializeField] private float spawnTime;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("TubePool").TryGetComponent(out objectPool);
        StartCoroutine(SpawnTube_co());
    }

    private void SetRandomPosition()
    {
        randomNum = Random.Range(9f, 20f);
    }

    private IEnumerator SpawnTube_co()
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnTime);
        while (true)
        {
            SetRandomPosition();
            objectPool.GetObject().transform.position = new Vector3(transform.position.x, randomNum, transform.position.z);
            yield return wfs;
        }
    }
}
