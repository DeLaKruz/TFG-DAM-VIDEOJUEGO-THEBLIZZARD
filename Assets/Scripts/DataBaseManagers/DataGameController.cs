using System;
using System.ComponentModel;
using System.Net;
using System.Security.AccessControl;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
/**
* *Este script se una tanto para el objeto de la escena que gestiona los guardados como para aplicarselo a los
* *puntos de guardado, la diferencia es que los puntos tendrán colliders que interactúan con el jugador.
* *Esto se hizo para optimizar los scripts.
*/
public class DataGameController : MonoBehaviour
{
    public static DataGameController instance; //Se declara como instancia para poder llamar sus métodos y variables de manera cómoda.
    public GameObject player;
    public string saveDataFile;
    string databtwscn;
    //private bool canSave; //booleano para saber si podemos guardar.
    public bool changeOfScene; //booleano que indica si estamos cambiando de pantalla para saber el fichero que escoger.
    public DataGame dataGame = new DataGame(); //Creamos una variable con la clase DataGame para guardar esos datos.


    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject); //Esto hará que al cambiar de escenas el Objeto que contiene el script se intercambie y no se destruya.
        saveDataFile = Application.dataPath + "/gameData.json"; //archivo de guardado general.
        databtwscn = Application.dataPath + "/databtwscn.json"; //archivo de guardado entre escenas.
    }

    // La clase update se usa para comprobar cuando se pulsan alguna de las siguientes teclas.
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(canSave);
        }

        // !Estas teclas de aquí solo se usan en versión de prueba, en el juego original no estarán disponibles.
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(LoadData());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            canSave = true;
            SaveData();
        }*/
    }

    /**
    * !En el codigo a continuación hay dos claras diferencias. Uno de los métodos se usa para cargar toda la partida.
    */

    //Datos que se cargan en ambos métodos de carga.
    private void dataToLoad()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerHealthController>().currenthealth = dataGame.life;
        player.GetComponent<PlayerController>().doubleJumpPower = dataGame.doubleJump;
        LevelManager.instance.gemsCollected = dataGame.money;
    }

    // Este método será el que se llame con el booleano desde donde se necesite, indicando si es un cambio de escena o no.
    public void callLoad(bool changeOfScene)
    {
        if (changeOfScene)
        {
            StartCoroutine(LoadScene());
        }
        else
        {
            StartCoroutine(LoadData());
        }
    }

    //El método SaveData guarda todos los datos de la partida.
    public void SaveData()
    {
            player = GameObject.FindGameObjectWithTag("Player");
            DataGame newData = new DataGame()
            {
                position = player.transform.position,
                life = player.GetComponent<PlayerHealthController>().currenthealth,
                money = LevelManager.instance.gemsCollected,
                doubleJump = player.GetComponent<PlayerController>().doubleJumpPower,
                currentScene = SceneManager.GetActiveScene().buildIndex
            };

            string JSONstring = JsonUtility.ToJson(newData);
            File.WriteAllText(saveDataFile, JSONstring);
            ChestLogic.CambiarEstadoGuardarCofres(true);
    }

    public void deathDataLoad()
    {
        string content = File.ReadAllText(saveDataFile);
        dataGame = JsonUtility.FromJson<DataGame>(content);
        LevelManager.instance.gemsCollected = dataGame.money;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerHealthController>().currenthealth = 6;

        player.transform.position = dataGame.position;
    }

    /**
    * ?Corrutina usada para cargar toda la partida. (Posición, escena, vida, dinero y poderes)
    */
    private IEnumerator LoadData()
    {
        if (File.Exists(saveDataFile))
        {
            string content = File.ReadAllText(saveDataFile);
            dataGame = JsonUtility.FromJson<DataGame>(content);

            var asyncOP = SceneManager.LoadSceneAsync(dataGame.currentScene);
            player = GameObject.FindGameObjectWithTag("Player");
            while (!asyncOP.isDone)
            {
                yield return null;
            }

            dataToLoad();
            player.transform.position = dataGame.position;
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
            DataGame newData = new DataGame()
            {
                life = 6,
                money = 0,
                doubleJump = false,
                currentScene = 1,
                position = new Vector3(0.8f, -6.2f, -3f)
            };

            string JSONstring = JsonUtility.ToJson(newData);
            File.WriteAllText(saveDataFile, JSONstring);
            ChestLogic.CambiarEstadoGuardarCofres(true);
        }
    }

    // Este método solo guarda los datos necesarios entre escenas.
    public void SaveDataBetweenScenes()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DataGame newData = new DataGame()
        {
            life = player.GetComponent<PlayerHealthController>().currenthealth,
            money = LevelManager.instance.gemsCollected,
            doubleJump = player.GetComponent<PlayerController>().doubleJumpPower
        };

        string JSONstring = JsonUtility.ToJson(newData);
        File.WriteAllText(databtwscn, JSONstring);
    }

    /**
    * ?Corrutina usada para cargar los datos necesarios al cambiar de escena. (vida, dinero y poderes)
    */
    private IEnumerator LoadScene()
    {
        yield return null;
        if (File.Exists(databtwscn))
        {
            string content = File.ReadAllText(databtwscn);
            dataGame = JsonUtility.FromJson<DataGame>(content);
            dataToLoad();
            UIController.instance.fadeFromBlack(false);
            UIController.instance.textoScene.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("El archivo no existe");
        }
    }
}