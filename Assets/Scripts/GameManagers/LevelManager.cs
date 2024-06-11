using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que gestiona el nivel del juego
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Instancia estática para el patrón Singleton
    public float waitToRespawn; // Tiempo de espera antes de respawnear al jugador
    public int gemsCollected; // Número de gemas recogidas
    private Animator anim; // Referencia al componente Animator
    public bool isPreviousScene;

    void Awake()
    {
        // Asignar la instancia estática
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Corrutina que se ejecuta al inicio
    IEnumerator Start()
    {
        // Espera un frame para asegurarte de que todos los objetos estén inicializados
        yield return null;

        // Buscar todos los cofres en la escena y comprobar su estado
        ChestLogic[] cofres = FindObjectsOfType<ChestLogic>();
        foreach (ChestLogic cofre in cofres)
        {
            cofre.ComprobarAbiertoPermanentemente();
        }

        // Buscar el objeto con la etiqueta "ChangeScene"
        SetPositionOnPreviousScene();

        //! UIController.instance.fadeScreen.color = new Color(UIController.instance.fadeScreen.color.r, UIController.instance.fadeScreen.color.g, UIController.instance.fadeScreen.color.b, 1f);
        // UIController.instance.fadeFromBlack();
    }

    // Método para respawnear al jugador
    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    public void SetPositionOnPreviousScene()
    {
        if (isPreviousScene)
        {
            GameObject changeSceneObject = GameObject.FindGameObjectWithTag("ChangeScene");
            // Si se encuentra el objeto "ChangeScene", mover al jugador a una nueva posición
            if (changeSceneObject != null)
            {
                // Calcular una nueva posición en el punto más alto del collider del objeto "ChangeScene"
                Collider2D changeSceneCollider = changeSceneObject.GetComponent<Collider2D>();
                if (changeSceneCollider != null)
                {
                    Vector3 newPosition = changeSceneCollider.bounds.max;
                    newPosition.x -= 6f; // Modifica esto según la distancia que quieras mover al jugador a la izquierda
                    newPosition.z = PlayerController.instance.transform.position.z;

                    // Mover al jugador a la nueva posición
                    PlayerController playerController = PlayerController.instance;
                    // Comprobar si el jugador es nulo antes de moverlo
                    if (playerController != null)
                    {
                        playerController.transform.position = newPosition;
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró el PlayerController.");
                    }
                }
                else
                {
                    Debug.LogWarning("El objeto 'ChangeScene' no tiene un collider.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el objeto con la etiqueta 'ChangeScene'.");
            }
        }
    }


    // Corrutina para gestionar el respawn del jugador
    IEnumerator RespawnCo()
    {
        if (PlayerController.instance != null && PlayerHealthController.instance.currenthealth > 0)
        {
            // Desactivar al jugador, esperar y luego reactivarlo en el punto de respawn
            PlayerController.instance.gameObject.SetActive(false);
            yield return new WaitForSeconds(waitToRespawn);
            PlayerController.instance.gameObject.SetActive(true);
            PlayerController.instance.transform.position = new Vector3(CheckPointController.instance.spawnPoint.x, CheckPointController.instance.spawnPoint.y, -4f);

            // Opcionalmente, reiniciar enemigos en la escena
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                // !enemy.instance.endAnim(true); TERMINAR
            }
            SetPositionOnPreviousScene();
        }
        else if (PlayerHealthController.instance.currenthealth <= 0)
        {
            PlayerHealthController.instance.currenthealth = PlayerHealthController.instance.maxhealth;
        }
        else
        {
            Debug.LogWarning("La instancia de PlayerController es nula. No se puede respawnear al jugador.");
        }
    }
}