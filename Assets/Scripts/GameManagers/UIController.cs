using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Unity.IO;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public UnityEngine.UI.Image heart1, heart2, heart3; // Corazones del UI.
    public Sprite heartfull, heartempty, hearthalf; // Imágenes de la vida llena, vacía y a mitad.

    public UnityEngine.UI.Image fadeScreen; // Pantalla para el desvanecimiento.
    public GameObject UICanvas;
    public Text textoScene;

    public bool isBack;
    public float fadeSpeed = 4f; //Velocidad del desvanecimiento
    public Text gemText; // Texto del dinero.
    private bool shouldFadeToBlack, shouldFadeFromBlack; //Variables para saber si hacemos o deshacemos el fundido en negro.


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optionally, keep this instance persistent between scenes
        }
    }
    // Start is called before the first frame update
    void Start()
    // Al comenzar se hace un fundido y se actualiza el dinero.
    {
        fadeFromBlack(false);
        UpdateGemCount();
    }

    // Se comprueba si debe hacer o deshacer el fundido, lo realiza según las variables y actualiza el dinero.
    void Update()
    {
        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
            }
        }
        UpdateGemCount();
    }

    //Actualiza la vida gráficamente, según la que dispongamos.
    public void UpdateHealthDisplay()
    {
        switch (PlayerHealthController.instance.currenthealth)
        {
            case 6:
                heart1.sprite = heartfull;
                heart2.sprite = heartfull;
                heart3.sprite = heartfull;
                break;
            case 5:
                heart1.sprite = heartfull;
                heart2.sprite = heartfull;
                heart3.sprite = hearthalf;
                break;
            case 4:
                heart1.sprite = heartfull;
                heart2.sprite = heartfull;
                heart3.sprite = heartempty;
                break;
            case 3:
                heart1.sprite = heartfull;
                heart2.sprite = hearthalf;
                heart3.sprite = heartempty;
                break;
            case 2:
                heart1.sprite = heartfull;
                heart2.sprite = heartempty;
                heart3.sprite = heartempty;
                break;
            case 1:
                heart1.sprite = hearthalf;
                heart2.sprite = heartempty;
                heart3.sprite = heartempty;
                break;
            case 0:
                heart1.sprite = heartempty;
                heart2.sprite = heartempty;
                heart3.sprite = heartempty;
                break;
            default:
                heart1.sprite = heartempty;
                heart2.sprite = heartempty;
                heart3.sprite = heartempty;
                break;
        }
    }

    // Cambia el valor del dinero por las gemas totales recogidas.
    public void UpdateGemCount()
    {
        gemText.text = LevelManager.instance.gemsCollected.ToString();
    }

    //Estos dos métodos ajustan el valor de las variables para el fundido.
    public void fadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }
    public void fadeFromBlack(bool isDeath)
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
        if (isDeath)
        {
            DataGameController.instance.deathDataLoad();
        }
    }

    public void DestroyObjects()
    {
        isBack = true;
        UICanvas.SetActive(false);
    }
}