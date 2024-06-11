using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private bool animEnd = false; // Indica si la animación ha terminado.
    [Range(0, 100)] public float chanceToDrop; // Probabilidad de soltar objetos al destruir un enemigo.
    private Collider2D currentCollider;
    public GameObject collectible1, collectible2; // Objetos que puede soltar el enemigo.
    private float ctd; // Probabilidad de soltar objetos (Chance To Drop).

    // Método que se llama cuando otro Collider2D entra en este Collider2D.
    private void OnTriggerEnter2D(Collider2D other)
    {
        currentCollider = other; // Guardar el Collider2D actual.

        // Verificar si el otro Collider2D pertenece a un enemigo.
        if (other.CompareTag("Enemy"))
        {
            PlayerController.instance.Bounce(); // Hacer rebotar al jugador.

            // Obtener el Animator y el script del enemigo.
            Animator enemyAnimator = other.transform.parent?.GetComponentInParent<Animator>();
            EnemyControllerMS enemy = other.transform.parent?.GetComponentInParent<EnemyControllerMS>();

            // Verificar si el enemigo no es nulo.
            if (enemy != null)
            {
                enemy.enemyLife--; // Reducir la vida del enemigo.
                // Verificar si la vida del enemigo es menor o igual a cero y si tiene Animator.
                if (enemy.enemyLife <= 0 && enemyAnimator != null)
                {
                    enemyAnimator.SetBool("lifeIsEmpty", true); // Establecer la animación de vida vacía.

                    // Seleccionar aleatoriamente si se deben soltar objetos.
                    float dropSelect = Random.Range(0, 100f);

                    // Desactivar el script de daño del enemigo.
                    DamagePlayer damage = other.transform.parent.GetComponentInParent<DamagePlayer>();
                    if (damage != null)
                    {
                        damage.enabled = false;
                    }

                    // Verificar si se debe soltar un objeto basado en la probabilidad.
                    if (dropSelect <= chanceToDrop / 2) // 50% de probabilidad para cada objeto
                    {
                        Instantiate(collectible1, other.transform.position, other.transform.rotation); // Instanciar objeto 1.
                    }
                    else if (dropSelect <= chanceToDrop) // El resto de la probabilidad es para el segundo objeto
                    {
                        Instantiate(collectible2, other.transform.position, other.transform.rotation); // Instanciar objeto 2.
                    }
                }
            }

            AudioManager.instance.PlaySFX(2); // Reproducir efecto de sonido.
        }

        // Verificar si el otro Collider2D pertenece a un cofre.
        if (other.CompareTag("Chest"))
        {
            Animator chestAnimator = other.transform.GetComponent<Animator>(); // Obtener el Animator del cofre.
            ChestLogic chestLogic = other.transform.GetComponent<ChestLogic>(); // Obtener el script del cofre.

            // Verificar si el Animator y el script del cofre no son nulos y si el cofre no está abierto.
            if (chestAnimator != null && chestLogic != null && !chestLogic.isOpened)
            {
                PlayerController.instance.Bounce(); // Hacer rebotar al jugador.
                chestAnimator.SetBool("isOpened", true); // Establecer la animación de abrir cofre.
                chestLogic.AbrirCofre(); // Abrir el cofre.
                int moneyToDrop = Random.Range(5, 15); // Generar una cantidad aleatoria de dinero a soltar.
                AudioManager.instance.PlaySFX(5); // Reproducir efecto de sonido.
                AudioManager.instance.PlaySFX(6); // Reproducir efecto de sonido.
                LevelManager.instance.gemsCollected += moneyToDrop; // Añadir el dinero recogido al contador del nivel.
                AudioManager.instance.PlaySFX(7); // Reproducir efecto de sonido.
            }
        }

        // Verificar si la animación ha terminado para desactivar el objeto del Collider2D.
        if (animEnd)
        {
            currentCollider.transform.parent.gameObject.SetActive(false);
        }
    }
}