using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage; //Numero de daño que se aplica al jugador.

    /**Si el jugador entra en contacto con el objeto que tenga este script
    *se llamará a DealDamage con el daño que se otorgue según el objeto/enemigo.
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DealDamage(damage);
        }
    }
}
