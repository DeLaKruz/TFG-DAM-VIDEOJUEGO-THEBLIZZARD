using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Animator anim;
    public float bounceForce = 15f; // Fuerza del impulso
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el jugador entra en contacto con el muelle se modifica su posición en "y" y se anima el muelle.
        if(other.tag == "Player"){
            PlayerController.instance.hero.velocity = new Vector2(PlayerController.instance.hero.velocity.x, bounceForce);
            anim.SetTrigger("Bounce");
        }
    }
}
