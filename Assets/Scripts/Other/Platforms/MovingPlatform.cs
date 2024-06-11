using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para controlar una plataforma móvil que se mueve entre varios puntos.
public class MovingPlatform : MonoBehaviour
{
    public Transform[] points; // Array de puntos entre los que se moverá la plataforma.
    public float moveSpeed; // Velocidad de movimiento de la plataforma.
    public int currentPoint; // Índice del punto actual hacia el que se dirige la plataforma.

    public Transform platform; // Transformador de la plataforma.

    // Método Update se llama una vez por frame.
    void Update()
    {
        // Movimiento de la plataforma hacia el punto actual.
        platform.position = Vector3.MoveTowards(platform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

        // Si la plataforma está cerca del punto actual, pasar al siguiente punto.
        if (Vector3.Distance(platform.position, points[currentPoint].position) < .05f)
        {
            currentPoint++; // Incrementar el índice del punto actual.

            // Si hemos alcanzado el último punto, volver al primero para crear un movimiento cíclico.
            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }
    }
}