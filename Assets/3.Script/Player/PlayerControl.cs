using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    public AudioClip deathClip;
    public AudioClip jumpClip;

    private bool isDead;
    private readonly int maxDeathCount = 3;
    private int deathCount = 1;
    public int DeathCount
    {
        get { return deathCount; }
        set
        {
            if(value >= maxDeathCount)
            {
                deathCount = maxDeathCount;
            }
            else
            {
                deathCount = value;
            }
        }
    }

    private Rigidbody playerRigid;
    private Animator animator;
    private AudioSource playerAudio;
    private SphereCollider birdCollider;
    private List<Transform> birdRenderer;

    [SerializeField] private float invincibleTime;
    public bool isInvincible { get; private set; } = false;

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
                SoundManager.Instance.PlayJump();
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
                    playerRigid.velocity *= 0.5f;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (transform.position.y >= 26)
        {
            transform.position = new Vector3(transform.position.x, 26, transform.position.z);
        }
        else if (transform.position.y <= 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Tube") || coll.gameObject.CompareTag("Ground"))
        {
            if (!isDead && deathCount > 0)
            {
                TakeDamage();
            }
        }
        //태그 변하는 버그 임시조치
        //if (!isDead && deathCount > 0)
        //{
        //    TakeDamage();
        //}
    }

    private void TakeDamage()
    {
        if (!isDead && DeathCount > 0)
        {
            DeathCount--;
            UIManager.instance.isPlayerLifeIncrease(false);
            if (DeathCount <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvincibleBird_co());
            }
        }
    }

    private void Die()
    {
        isDead = true;
        SoundManager.Instance.PlayDeath();
        playerRigid.velocity = Vector3.zero;
    
        UIManager.instance.gameOver();
    }

    private IEnumerator InvincibleBird_co()
    {
        isInvincible = true;
        birdCollider.isTrigger = true;
        Coroutine runningCoroutine = StartCoroutine(BlinkBird_co());
        yield return new WaitForSeconds(invincibleTime);
        StopCoroutine(runningCoroutine);
        BlickBird(true);
        birdCollider.isTrigger = false;
        isInvincible = false;
    }

    private IEnumerator BlinkBird_co()
    {
        while (true)
        {
            BlickBird(false);
            yield return new WaitForSeconds(0.2f);
            BlickBird(true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void BlickBird(bool isActive)
    {
        for (int i = 0; i < birdRenderer.Count; i++)
        {
            birdRenderer[i].gameObject.SetActive(isActive);
        }
    }
}
