using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject[] principalButtons;
    public GameObject[] startButtons;
    public GameObject panel;
    public GameObject UICanvas;

    public GameObject UICnv;
    //public GameObject LevelMngr;
    //public GameObject DataController;
    public GameObject AudioMngr;

    void Start()
    {
        OptionsMenu.instance.LoadOptions();
        OptionsMenu.instance.CloseOptionsMenu();
        UICanvas.SetActive(false);

        if (UIController.instance.isBack)
        {
            //Destroy(UICanvas);
            Debug.Log("destroyed");
            //Destroy(LevelMngr);
            //Destroy(DataController);
            // Destroy(AudioMngr);
        }
    }

    public void StartGame()
    {
        // Carga la escena con el índice 1 en la build settings.
        SceneManager.LoadScene(1);

        // UIController.instance.fadeToBlack();
        // UIController.instance.fadeFromBlack();

        // Llama a la función callLoad del controlador de datos del juego para cargar los datos del juego.
        DataGameController.instance.callLoad(false);
        UICanvas.SetActive(true);
    }

    public void startNewGame()
    {
        SceneManager.LoadScene(1);
        string filePath = Application.dataPath + "/gameData.json";
        bool fileExists = File.Exists(filePath);
        if (fileExists)
        {
            File.Delete(filePath);
        }
        DataGameController.instance.callLoad(false);
        UICanvas.SetActive(true);
    }

    public void changeButtons(bool start)
    {
        foreach (GameObject obj in principalButtons)
        {
            obj.SetActive(!start);
        }

        string filePath = Application.dataPath + "/gameData.json";
        bool fileExists = File.Exists(filePath);
        int index = 0;
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

    public void showOptions()
    {
        panel.SetActive(true);
    }

    // Método para salir del juego.
    public void QuitGame()
    {
        // Cierra la aplicación.
        Application.Quit();
    }
}