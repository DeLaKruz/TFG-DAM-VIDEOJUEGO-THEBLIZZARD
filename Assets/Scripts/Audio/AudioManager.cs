using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource[] Music;
    public AudioSource[] SFX;
    private float walkWait;
    public float maxWalkWait = 0.4f;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex != 0)
            if (PlayerController.instance.isWalking)
            {
                walkWait -= Time.deltaTime;
            }
            else
            {
                if (PlayerController.instance.isGrounded)
                {
                    walkWait = 0;
                }
            }
    }

    public void PlaySFX(int soundToPlay)
    {
        if (soundToPlay == 4)
        {
            if (walkWait <= 0)
            {
                SFX[soundToPlay].Stop();
                SFX[soundToPlay].pitch = Random.Range(.9f, 1.1f);
                SFX[soundToPlay].Play();
                walkWait = maxWalkWait;
            }
        }
        else if (soundToPlay != 4)
        {
            Debug.Log(soundToPlay);
            SFX[soundToPlay].Stop();
            SFX[soundToPlay].pitch = Random.Range(.9f, 1.1f);
            SFX[soundToPlay].Play();
        }
    }

    public void PlayMusic(int soundToPlay)
    {
            SFX[soundToPlay].Stop();
            SFX[soundToPlay].Play();
    }
}
