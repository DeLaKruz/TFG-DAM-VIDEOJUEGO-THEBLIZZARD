using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    // Objeto que contiene los textos que se mostrarán al interactuar
    public Texts texts;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entra en contacto tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Encuentra todas las instancias de DialogController en la escena
            DialogController[] dialogControllers = FindObjectsOfType<DialogController>();
            // Activa el cartel de diálogo para cada DialogController encontrado
            foreach (DialogController dialogController in dialogControllers)
            {
                dialogController.ActivateCartel(texts);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Encuentra todas las instancias de DialogController en la escena
        DialogController[] dialogControllers = FindObjectsOfType<DialogController>();
        // Cierra el cartel de diálogo para cada DialogController encontrado
        foreach (DialogController dialogController in dialogControllers)
        {
            dialogController.CloseCartel();
        }
    }
}