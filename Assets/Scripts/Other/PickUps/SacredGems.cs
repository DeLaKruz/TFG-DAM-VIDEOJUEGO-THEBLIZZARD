using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredGems : MonoBehaviour
{
    // Tipo de gema (por ejemplo, 1 para doble salto, 2 para dash)
    public int gemType;
    
    // Objeto que contiene los textos que se mostrarán al recoger la gema
    public Texts texts;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entra en contacto tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Realiza una acción diferente según el tipo de gema
            switch (gemType)
            {
                case 1:
                    // Otorga el poder de doble salto al jugador
                    PlayerController.instance.doubleJumpPower = true;
                    // Destruye el objeto de la gema
                    Destroy(gameObject);
                    // Muestra el texto asociado a la gema
                    playText();
                    break;
                case 2:
                    // Otorga los poderes de dash y deslizamiento en la pared al jugador
                    PlayerController.instance.dashpower = true;
                    PlayerController.instance.wallslidepower = true;
                    // Muestra el texto asociado a la gema
                    playText();
                    // Destruye el objeto de la gema
                    Destroy(gameObject);
                    break;
            }
        }
    }

    // Método para mostrar el texto asociado a la gema
    void playText()
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