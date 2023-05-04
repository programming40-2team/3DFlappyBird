using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    public AudioClip deathClip;
    public AudioClip jumpClip;

    private bool isDead;
    public int deathCount = 1;

    private Rigidbody playerRigid;
    public Animator animator;
    private AudioSource playerAudio;
    private SphereCollider birdCollider;
    private List<Transform> birdRenderer;

    [SerializeField] private float invincibleTime;

    [Header("점프력 조절")]
    [SerializeField][Range(10f, 50f)] private float jumpForce = 10f;
    private int jumpCount = 0;


    void Start()
    {
        birdRenderer = new List<Transform>();
        isDead = false;
        playerRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        birdCollider = GetComponent<SphereCollider>();
        foreach (Transform birdBody in transform)
        {
            if (birdBody.TryGetComponent(out Transform birdBodyGameobj))
            {
                birdRenderer.Add(birdBodyGameobj);
            }
        }
    }

    void Update()
    {
        animator.SetBool("Fly", true);

        if (isDead == false)
        {
            //마우스를 클릭할때마다 y축으로 AddForce 적용되어 점프
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerRigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                transform.Rotate(new Vector3(10, 0, 0));    //캐릭터의 몸통이 위를 향하는 각도 변화
                jumpCount++;

                if (jumpCount >= 2)
                {
                    transform.Rotate(new Vector3(-20, 0, 0)); //캐릭터의 몸통이 다시 아래를 향하는 각도 변화
                    jumpCount = 0;
                }
            }
            else
            {
                //마우스를 떼는 순간 속도의 y값이 양수라면 속도가 절반으로 => 적용해보니 너무 게임이 쉬워지는듯한 단점
                if (Input.GetKeyUp(KeyCode.Mouse0) && playerRigid.velocity.y > 0)
                {
                    playerRigid.velocity = playerRigid.velocity * 0.5f;
                }
            }
        }

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Tube"))
        {
            if (!isDead && deathCount > 0)
            {
                deathCount--;
                if (deathCount <= 0)
                {
                    Die();
                }
                else
                {
                    StartCoroutine(InvincibleBird_co());
                }
            }
        }
        if (coll.gameObject.CompareTag("Ground"))
        {
            if (!isDead && deathCount > 0)
            {
                deathCount--;
                if (deathCount <= 0)
                {
                    Die();
                }
                else
                {
                    StartCoroutine(InvincibleBird_co());
                }
            }
        }
    }

    private void Die()
    {
        isDead = true;
        playerRigid.velocity = Vector3.zero;
        //playerAudio.clip = deathClip;
        // playerAudio.Play();
        UIManager.instance.gameOver();
    }

    private IEnumerator InvincibleBird_co()
    {
        birdCollider.isTrigger = true;
        Coroutine runningCoroutine = StartCoroutine(BlinkBird_co());
        yield return new WaitForSeconds(invincibleTime);
        StopCoroutine(runningCoroutine);
        birdCollider.isTrigger = false;
    }

    private IEnumerator BlinkBird_co()
    {
        while (true)
        {
            for (int i = 0; i < birdRenderer.Count; i++)
            {
                birdRenderer[i].gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < birdRenderer.Count; i++)
            {
                birdRenderer[i].gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
