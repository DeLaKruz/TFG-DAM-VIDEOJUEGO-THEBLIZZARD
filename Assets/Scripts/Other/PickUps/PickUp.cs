using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool isGem, isHeal; // Variables para determinar si el pickUp es gema o vida.
    private bool isCollected; // Determina si el pickUp ya fue recogido.
    public GameObject pickUpEffect; // Efecto al recogerlo.

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si entramos en contacto con el pickUp y no ha sido recogido.
        if (other.CompareTag("Player") && !isCollected)
        {
            // En caso de ser una gema.
            if (isGem)
            {
                // Sumamos entre 1 y 5 de dinero, reproducimos su sonido, actualizamos la parte gráfica y lo destruimos.
                int moneyToDrop = Random.Range(1, 5);
                LevelManager.instance.gemsCollected += moneyToDrop;
                AudioManager.instance.PlaySFX(7);
                UIController.instance.UpdateGemCount();
                isCollected = true;
                Instantiate(pickUpEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }

            // En caso de ser un corazón.
            if (isHeal)
            {
                // En caso de no tener la vida llena, llamamos a HealPlayer de PlayerHealthController y destruimos el corazón.
                if (PlayerHealthController.instance.currenthealth != PlayerHealthController.instance.maxhealth)
                {
                    PlayerHealthController.instance.HealPlayer();
                    isCollected = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
