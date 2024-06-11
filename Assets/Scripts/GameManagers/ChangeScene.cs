using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public static ChangeScene instance;
    private float fadeTime; // Tiempo de desvanecimiento.
    private bool startCount; // Indica si el contador de desvanecimiento ha comenzado.
    private int sceneIndex; // Índice de la escena actual.
    private float fadeTimeMax = 1f; // Tiempo máximo de desvanecimiento.
    public bool previousScene; // Indica si debemos cambiar a la escena anterior.

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Establece el tiempo de desvanecimiento a su valor máximo.
        fadeTime = fadeTimeMax;
        // Obtiene el índice de la escena actual.
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        // Si el contador de desvanecimiento ha comenzado, reduce el tiempo de desvanecimiento.
        if (startCount)
        {
            fadeTime -= Time.deltaTime;
        }

        // Si el tiempo de desvanecimiento ha llegado a cero hace lo siguiente:
        if (fadeTime <= 0)
        {
            // Detiene el contador.
            startCount = false;
            // Desactiva la pausa del juego.
            PauseMenu.instance.PauseUnpause();
            // Guarda los datos necesarios entre escenas.
            DataGameController.instance.SaveDataBetweenScenes();

            // Cambia a la escena anterior o posterior según previousScene.
            if (previousScene)
            {
                SceneManager.LoadScene(sceneIndex - 1);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex + 1);
            }

            // Restablece el tiempo de desvanecimiento a su valor máximo.
            fadeTime = fadeTimeMax;
            // Coloca al jugador en la posición correcta y carga los datos guardados.
            LevelManager.instance.RespawnPlayer();
            // Inicia la transición de desvanecimiento (comentado por ser decorativo).
            // UIController.instance.fadeFromBlack();
            // Carga los datos guardados para el nuevo jugador.
            DataGameController.instance.callLoad(true);
        }
    }

    // Método llamado cuando otro objeto entra en contacto con el trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra en contacto es el jugador, inicia el proceso de cambio de escena.
        if (other.CompareTag("Player"))
        {
            UIController.instance.fadeToBlack();
            PauseMenu.instance.isPaused = true;
            startCount = true;
            LevelManager.instance.isPreviousScene = previousScene;
            UIController.instance.textoScene.gameObject.SetActive(true);

            int indiceDeLaEscena = SceneManager.GetActiveScene().buildIndex;


            if (previousScene)
            {
                indexText(indiceDeLaEscena - 1);
            }
            else
            {
                indexText(indiceDeLaEscena + 1);
            }
        }
    }

    public void indexText(int indiceDeLaEscena)
    {
        switch (indiceDeLaEscena)
        {
            case 1:
                UIController.instance.textoScene.text = "Bosque Profundo";
                AudioManager.instance.PlayMusic(0);
                break;
            case 2:
                UIController.instance.textoScene.text = "Afueras del Bosque";
                break;
            case 3:
                UIController.instance.textoScene.text = "Ciudad Derruida";
                break;
            case 4:
                UIController.instance.textoScene.text = "Parte trasera de la Ciudad";
                break;
            case 5:
                UIController.instance.textoScene.text = "VENTISCA ETERNA";
                AudioManager.instance.PlayMusic(1);
                PlayerController.instance.hero.velocity = new Vector2(-13.8f, -3f);
                break;
        }
    }
}