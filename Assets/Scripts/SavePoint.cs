using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private bool canSave;
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que contiene este script tiene un collider, y entra en contacto con el jugador podremos guardar.
        if (other.CompareTag("Player"))
        {
            canSave = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSave = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            DataGameController.instance.SaveData();
        }
    }
}
