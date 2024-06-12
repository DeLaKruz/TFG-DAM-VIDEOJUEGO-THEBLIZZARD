using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController instance;
    
    private Animator anim;
    
    // Cola para almacenar las líneas de diálogo
    private Queue<string> dialogqueue = new Queue<string>();
    
    // Corutina que se utiliza para mostrar el texto caracter por caracter
    private Coroutine typingCoroutine;
    
    // Indica si actualmente se está escribiendo texto
    private bool isTyping = false;
    
    // Objeto que contiene el texto a mostrar
    Texts text;
    
    // Indica si el diálogo es del jefe final
    public bool finalboss;
    
    // Referencia al componente Text de la pantalla donde se muestra el diálogo
    [SerializeField] Text screenText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Método para activar el cartel de diálogo con el texto proporcionado
    public void ActivateCartel(Texts textObject)
    {
        if (anim == null || textObject == null)
        {
            return;
        }

        // Activa la animación del cartel de diálogo
        anim.SetBool("Cartel", true);
        
        // Asigna el objeto de texto
        text = textObject;
        
        // Activa el texto
        ActivateText();
    }

    void Update()
    {
        // Si se presiona la tecla W y no se está escribiendo, muestra el siguiente texto
        if (Input.GetKeyDown(KeyCode.W) && !isTyping)
        {
            NextText();
        }
    }

    // Método para activar y mostrar el texto en el cartel
    public void ActivateText()
    {
        // Limpia la cola de diálogo
        dialogqueue.Clear();
        
        // Encola cada línea de texto en la cola de diálogo
        foreach (string savetext in text.textsArray)
        {
            dialogqueue.Enqueue(savetext);
        }

        // Si hay textos en la cola, muestra el siguiente texto
        if (dialogqueue.Count > 0)
        {
            NextText();
        }
    }

    public void NextText()
    {
        // Si la cola de diálogo está vacía
        if (dialogqueue.Count == 0)
        {
            // Si es el jefe final, se indica que el texto de la batalla terminó
            if (finalboss)
            {
                FinalBoss.instance.endBattleText = true;
                FinalBoss.instance.canStartBattle = true;
            }
            // Cierra el cartel de diálogo
            CloseCartel();
            return;
        }

        // Si actualmente se está escribiendo, se hace return
        if (isTyping)
        {
            return;
        }

        // Desencola el siguiente texto
        string actualText = dialogqueue.Dequeue();

        // Si ya hay una corutina de escritura en ejecución, la detiene
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Inicia una nueva corutina para mostrar el texto caracter por caracter
        typingCoroutine = StartCoroutine(ShowCharacters(actualText));
    }

    // Método para cerrar el cartel de diálogo
    public void CloseCartel()
    {
        anim.SetBool("Cartel", false);
    }

    // Corutina para mostrar el texto caracter por caracter
    IEnumerator ShowCharacters(string textToShow)
    {
        // Marca que se está escribiendo
        isTyping = true;
        Debug.Log("Started typing: " + textToShow);
        
        // Limpia el texto en pantalla
        screenText.text = "";
        
        // Agrega cada caracter del texto uno por uno con un pequeño retardo
        foreach (char character in textToShow.ToCharArray())
        {
            screenText.text += character;
            yield return new WaitForSeconds(0.02f);
        }
        
        // Marca que ha terminado de escribir
        isTyping = false;
    }
}