using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public bool killzone; // Determina si se trata de una zona de muerte o no.
    public Transform spikesColl;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        // Compara si el jugador ha entrado en contacto
        if (other.CompareTag("Player"))
        {
            // En caso de ser killzone hace reaparecer al jugador en el último checkpoint.
            if (killzone)
            {
                LevelManager.instance.RespawnPlayer();
            }
        }
    }
}
