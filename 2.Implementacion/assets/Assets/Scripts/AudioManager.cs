using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    // Start is called before the first frame update
    [SerializeField] AudioClip musicGame;
    [SerializeField] AudioClip musicMenu;
    public AudioClip jump;
    public AudioClip death;
    public AudioClip menu;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        musicSource.clip = musicMenu;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scene_Start" || SceneManager.GetActiveScene().name == "Scene_Final")
        {
            musicSource.clip = musicMenu;
            musicSource.loop = true;
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
            if (Input.GetButtonDown("Jump"))
            {
                PlaySFX(menu);
            }
        }
        else
        {
            musicSource.clip = musicGame;
            musicSource.loop = true;
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }

    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == death && sfxSource.isPlaying)
        {
            return;
        }
        sfxSource.PlayOneShot(clip);
    }
}
