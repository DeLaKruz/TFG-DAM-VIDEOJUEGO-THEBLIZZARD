using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script controla el comportamiento de un enemigo volador.
public class FlyingEnemy : MonoBehaviour
{
    public Transform[] points; // Array de puntos de movimiento entre los que el enemigo se desplaza.
    public float moveSpeed; // Velocidad de movimiento del enemigo.
    private int currentPoint; // Índice del punto actual hacia el que se mueve el enemigo.
    public SpriteRenderer theSR; // Referencia al SpriteRenderer del enemigo.
    public float distanceToAtackPlayer, chaseSpeed; // Distancia para detectar al jugador y velocidad de persecución.
    private Vector3 attackTarget; // Objetivo de ataque del enemigo.

    public float waitAfterAttack; // Tiempo de espera después de un ataque.
    private float attackCounter; // Contador para el tiempo de espera después de un ataque.

    // Start se llama antes de la primera actualización del frame
    void Start()
    {
        // Desvincula los puntos de movimiento del enemigo para que no se muevan con él.
        for (int i = 0; i < points.Length; i++)
        {
            points[i].parent = null;
        }
    }

    void Update()
    {
        // Maneja el tiempo de espera después de un ataque.
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        else
        {
            // Si el jugador está fuera del rango de ataque, el enemigo se mueve entre los puntos.
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > distanceToAtackPlayer)
            {
                // Mueve al enemigo hacia el punto actual.
                transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

                // Si el enemigo alcanza el punto actual, actualiza el punto objetivo al siguiente.
                if (Vector3.Distance(transform.position, points[currentPoint].position) < .05f)
                {
                    currentPoint++;

                    // Si el enemigo ha alcanzado el último punto, reinicia al primer punto.
                    if (currentPoint >= points.Length)
                    {
                        currentPoint = 0;
                    }
                }

                // Ajusta la dirección del sprite del enemigo según el punto objetivo.
                if (transform.position.x < points[currentPoint].position.x)
                {
                    theSR.flipX = false;
                }
                else
                {
                    theSR.flipX = true;
                }
            }
            else
            {
                // Si el jugador está dentro del rango de ataque, el enemigo lo persigue.
                if (attackTarget == Vector3.zero)
                {
                    attackTarget = PlayerController.instance.transform.position;
                }

                // Mueve al enemigo hacia el objetivo de ataque.
                transform.position = Vector3.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);

                // Si el enemigo alcanza el objetivo de ataque, inicia el contador de espera después del ataque.
                if (Vector3.Distance(transform.position, attackTarget) <= .1f)
                {
                    attackCounter = waitAfterAttack;
                    attackTarget = Vector3.zero;
                }
            }
        }
    }
}