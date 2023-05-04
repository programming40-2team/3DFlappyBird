using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    public AudioClip deathClip;
    public AudioClip jumpClip;

    private bool isDead;

    private Rigidbody playerRigid;
    public Animator animator;
    private AudioSource playerAudio;

    [Header("������ ����")]
    [SerializeField] [Range(10f, 50f)] private float jumpForce = 10f;
    private int jumpCount = 0;

    void Start()
    {
        isDead = false;
        playerRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }


    void Update()
    {
        animator.SetBool("Fly", true);

        if (isDead == false)
        {
            //���콺�� Ŭ���Ҷ����� y������ AddForce ����Ǿ� ����
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerRigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                transform.Rotate(new Vector3(10, 0, 0));    //ĳ������ ������ ���� ���ϴ� ���� ��ȭ
                jumpCount++;

                if (jumpCount >= 2)
                {
                    transform.Rotate(new Vector3(-20, 0, 0)); //ĳ������ ������ �ٽ� �Ʒ��� ���ϴ� ���� ��ȭ
                    jumpCount = 0;
                }
            }
            else
            {
                //���콺�� ���� ���� �ӵ��� y���� ������ �ӵ��� �������� => �����غ��� �ʹ� ������ �������µ��� ����
                if (Input.GetKeyUp(KeyCode.Mouse0) && playerRigid.velocity.y > 0)
                {
                    playerRigid.velocity = playerRigid.velocity * 0.5f;
                }
            }
        }
        Debug.Log(isDead);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Tube"))
        {
            Die();
        }
        if (coll.gameObject.CompareTag("Ground"))
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        playerRigid.velocity = Vector3.zero;
        playerAudio.clip = deathClip;
        playerAudio.Play();
    }
}