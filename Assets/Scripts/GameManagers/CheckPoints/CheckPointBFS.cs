using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBFS : MonoBehaviour
{
    /**Comparamos si el checkpoint entra en contacto con el jugador y llamamos al método
    *DeactivateCheckPoints para quitar el chekpoint actual y al setspawnpoint para establecer 
    *el nuevo checkpoint en la posición actual. Los métodos son de checkpointController 
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointController.instance.SetSpawnPoint(transform.position);
        }
    }
}