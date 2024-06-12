using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    // Referencias a los objetos de la interfaz del menú principal y de inicio
    public GameObject[] principalButtons; // Botones del menú principal
    public GameObject[] startButtons; // Botones del menú de inicio
    public GameObject panel; // Panel de opciones
    public GameObject UICanvas; // Canvas principal de la interfaz de usuario

    // Referencias a otros componentes del juego
    public GameObject UICnv;
    //public GameObject LevelMngr;
    //public GameObject DataController;
    public GameObject AudioMngr;

    void Start()
    {
        // Carga las opciones guardadas y cierra el menú de opciones al iniciar
        OptionsMenu.instance.LoadOptions();
        OptionsMenu.instance.CloseOptionsMenu();
        UICanvas.SetActive(false); // Desactiva el canvas principal de la interfaz de usuario

        // Si se vuelve al menú principal desde el juego, se realizan ciertas acciones
        if (UIController.instance.isBack)
        {
            // Destruir el UICanvas
            //Debug.Log("destroyed");
            // Destruir otros objetos del juego
            //Destroy(LevelMngr);
            //Destroy(DataController);
            // Destroy(AudioMngr);
        }
    }

    // Método para iniciar el juego
    public void StartGame()
    {
        // Carga la escena con el índice 1
        SceneManager.LoadScene(1);

        // Llama a la función callLoad del controlador de datos del juego para cargar los datos del juego
        DataGameController.instance.callLoad(false);
        UICanvas.SetActive(true); // Activa el canvas principal de la interfaz de usuario
    }

    // Método para iniciar un nuevo juego
    public void startNewGame()
    {
        SceneManager.LoadScene(1); // Carga la escena con el índice 1

        // Elimina el archivo de datos del juego si existe
        string filePath = Application.dataPath + "/gameData.json";
        bool fileExists = File.Exists(filePath);
        if (fileExists)
        {
            File.Delete(filePath);
        }

        // Llama a la función callLoad del controlador de datos del juego para cargar los datos del juego
        DataGameController.instance.callLoad(false);
        UICanvas.SetActive(true); // Activa el canvas principal de la interfaz de usuario
    }

    // Método para cambiar la visibilidad de los botones
    public void changeButtons(bool start)
    {
        // Activa o desactiva los botones del menú principal
        foreach (GameObject obj in principalButtons)
        {
            obj.SetActive(!start);
        }

        // Verifica si el archivo de datos del juego existe
        string filePath = Application.dataPath + "/gameData.json";
        bool fileExists = File.Exists(filePath);
        int index = 0;

        // Activa o desactiva los botones del menú de inicio según la existencia del archivo de datos del juego
        foreach (GameObject obj in startButtons)
        {
            if (index == 0 && !fileExists)
            {
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(start);
            }
            index++;
        }
    }

    // Método para mostrar el panel de opciones
    public void showOptions()
    {
        panel.SetActive(true); // Activa el panel de opciones
    }

    // Método para salir del juego
    public void QuitGame()
    {
        // Cierra la aplicación
        Application.Quit();
    }
}