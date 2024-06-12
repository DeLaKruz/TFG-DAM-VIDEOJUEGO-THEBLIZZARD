using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currenthealth, maxhealth; // Vida actual y máxima.
    public float invencibleTime; // Tiempo de invencibilidad.
    private float invencibleCounter; // Contador para la invencibilidad.

    private SpriteRenderer SpriteR;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    // Se establece la vida actual a la vida máxima.
    void Start()
    {
        currenthealth = maxhealth;
        SpriteR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Si el personaje es invencible se comienza la cuenta para desactivarlo.
        if (invencibleCounter > 0)
        {
            invencibleCounter -= Time.deltaTime;

            // Cuando acabe la cuenta se vuelve a poner el sprite del jugador normal.
            if (invencibleCounter <= 0)
            {
                SpriteR.color = new Color(SpriteR.color.r, SpriteR.color.g, SpriteR.color.b, 1f);
            }
        }
        // Actualiza la parte gráfica de la vida.
        UIController.instance.UpdateHealthDisplay();
    }

    // Método llamado para restar vida al jugador.
    public void DealDamage(int damage)
    {
        // Comprueba si no somos invencibles
        if (invencibleCounter <= 0)
        {
            // Resta a la vida el daño indicado cuando llamamos a la función.
            currenthealth -= damage;
            // Activa la animación de daño del jugador.
            PlayerController.instance.anim.SetTrigger("hurt");

            // Si el jugador muere activa su muerte
            if (currenthealth <= 0)
            {
                currenthealth = 0;
                Muerte.instance.activarMuerte();
            }
            else
            {
                // Si el jugador está vivo, se aplica la invencibilidad y se transparenta un poco al jugador (Para feedback).
                invencibleCounter = invencibleTime;
                SpriteR.color = new Color(SpriteR.color.r, SpriteR.color.g, SpriteR.color.b, .5f);
                // Recibe un impulso al obtener daño.
                PlayerController.instance.Knockback();
            }
        }
    }

    // Cura al jugador.
    public void HealPlayer()
    {
        // Suma uno de vida al jugador, a no ser que ya esté al máximo, que no hará nada.
        currenthealth++;
        if (currenthealth > maxhealth)
        {
            currenthealth = maxhealth;
        }
        // Se actualiza la parte gráfica de la vida.
        UIController.instance.UpdateHealthDisplay();
    }

    //Cuando entra en contacto con la plataforma se hace su objeto hijo para moverse con ella y cuando sale se desvincula.
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }
}
