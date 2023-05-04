using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ScoreTube = new List<GameObject>();
    [SerializeField]
    private float cameraMovingTime = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(FindObjectWithScore_Co));
    }

    IEnumerator FindObjectWithScore_Co()
    {
        while (true)
        {
            ScoreTube = GameObject.FindGameObjectsWithTag("Score").ToList();
            yield return new WaitForSeconds(4.0f);
        }

    }
    IEnumerator MoveTowards_Co()
    {

        if (ScoreTube.Count.Equals(0))
        {

        }
        else
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(transform.position.x, ScoreTube[0].transform.position.y, transform.position.z);
            float elapsedTime = 0;
            while (elapsedTime < cameraMovingTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / cameraMovingTime;
                t = Mathf.SmoothStep(0, 1, t); // 속도를 미세하게 조절
                Vector3 newPos = Vector3.Lerp(startPos, endPos, t);
                transform.position = newPos;
                yield return null;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Score"))
        {
            if (ScoreTube.Contains(other.gameObject))
            {
                ScoreTube.Remove(other.gameObject);
                StartCoroutine(nameof(MoveTowards_Co));
            }
        }
    }
}
