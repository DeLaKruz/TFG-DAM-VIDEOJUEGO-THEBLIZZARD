using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string mainMenu;
    public GameObject pauseScreen;
    public bool isPaused;
    public static PauseMenu instance;
    public GameObject panel;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Comprobamos si pulsamos el boton de menú (escape) y se llama PauseUnPause.
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        /**Comprueba si está pausado, si lo esta se pone a false, se desactiva la pantalla
        *de pausa y se establece el tiempo a velocidad normal.
        */
        if (isPaused)
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
        /**Comprueba si está pausado, si no lo esta se pone a false, se activa la pantalla
        *de pausa y se establece el tiempo en paralizado.
        */
        else
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    //Te devuelve al menú principal
    //!HACER
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void showOptions()
    {
        panel.SetActive(true);
    }
}
