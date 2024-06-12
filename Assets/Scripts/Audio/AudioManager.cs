using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    // Variables para almacenar las fuentes de audio de la música y los efectos de sonido
    public AudioSource[] Music;
    public AudioSource[] SFX;

    // Variables para controlar el tiempo entre los efectos de sonido de caminar
    private float walkWait;
    public float maxWalkWait = 0.4f;

    void Awake()
    {
        // Asegurarse de que solo haya una instancia de AudioManager
        instance = this;
        // Evitar que el AudioManager se destruya al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Obtener la escena activa actual
        Scene currentScene = SceneManager.GetActiveScene();
        
        // Verificar si la escena actual no es la inicial.
        if (currentScene.buildIndex != 0)
        {
            // Si el jugador está caminando, disminuir el temporizador walkWait.
            if (PlayerController.instance.isWalking)
            {
                walkWait -= Time.deltaTime;
            }
            else
            {
                // Si el jugador está en el suelo, reiniciar walkWait.
                if (PlayerController.instance.isGrounded)
                {
                    walkWait = 0;
                }
            }
        }
    }


    public void PlaySFX(int soundToPlay)
    {
        // Si el sonido a reproducir es el de caminar
        if (soundToPlay == 4)
        {
            // Si el temporizador ha llegado a cero, reproducir el sonido de caminar
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
            // Para otros efectos de sonido
            Debug.Log(soundToPlay);
            SFX[soundToPlay].Stop();
            SFX[soundToPlay].pitch = Random.Range(.9f, 1.1f);
            SFX[soundToPlay].Play();
        }
    }

    // Método para reproducir música
    public void PlayMusic(int soundToPlay)
    {
        // Detener cualquier música que se esté reproduciendo y reproducir la nueva música.
        Music[soundToPlay].Stop();
        Music[soundToPlay].Play();
    }
}