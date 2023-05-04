using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ScoreTube = new List<GameObject>();
    [SerializeField]
    private float cameraMovingTime = 4.0f;
    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(nameof(FindObjectWithScore_Co));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FindObjectWithScore_Co()
    {
        while (true)
        {
            ScoreTube.Clear();
            ScoreTube = GameObject.FindGameObjectsWithTag("Score").ToList();
            yield return new WaitForSeconds(4.0f);
        }

    }
    IEnumerator MoveTowards_Co()
    {
        yield return new WaitForSeconds(4.0f);
        if (ScoreTube.Count.Equals(0))
        {

        }
        else
        {
            float elapsedTime = 0;
            while (elapsedTime < cameraMovingTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / cameraMovingTime);
                //Vector3 newPos = new Vector3(transform.position.x, Mathf.Lerp(.position.y, endPoint.position.y, t), transform.position.z);
                //transform.position = newPos;
            }

        }

    }
}
