using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private AudioSource mAudioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip getHeart;
    [SerializeField] private AudioClip lostHeart;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip superStar;
    private AudioSource bgmPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        TryGetComponent(out mAudioSource);
        bgmPlayer = GameObject.FindGameObjectWithTag("BGMPlayer").GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgmPlayer = GameObject.FindGameObjectWithTag("BGMPlayer").GetComponent<AudioSource>();
    }
    
    public void UpdateBgmPlayer()
    {
        bgmPlayer = GameObject.FindGameObjectWithTag("BGMPlayer").GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        mAudioSource.PlayOneShot(clickSound);
    }
    public void PlayGetHeart()
    {
        mAudioSource.PlayOneShot(getHeart);
    }
    public void PlayLostHeart()
    {
        mAudioSource.PlayOneShot(lostHeart);
    }
    public void PlayJump()
    {
        mAudioSource.PlayOneShot(jump);
    }
    public void PlayDeath()
    {
        mAudioSource.PlayOneShot(death);
    }

    public void PlaySuperStar()
    {
        bgmPlayer.Pause();
        mAudioSource.clip = superStar;
        mAudioSource.Play();
    }

    public void StopSuperStar()
    {
        mAudioSource.Stop();
        bgmPlayer.Play();
    }
}
