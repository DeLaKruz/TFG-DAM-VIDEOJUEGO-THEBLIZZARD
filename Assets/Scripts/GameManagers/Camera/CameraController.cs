using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Transform del objetivo que la cámara seguirá.
    public Transform farBackground, middleBackground; // Transforms para los fondos distantes y medios.
    public Transform particles; // Transform para las partículas en la escena.
    public float minHeight, maxHeight, minX, maxX; // Límites de posición para la cámara.
    public bool itsRain; // Booleano para indicar si está lloviendo.

    private Vector2 lastPos; // Posición anterior de la cámara.

    void Start()
    {
        // Inicializa la última posición de la cámara con su posición actual.
        lastPos = transform.position;
    }

    // Update se llama una vez por frame.
    void Update()
    {
        // Calcula la nueva posición de la cámara, limitada dentro de los valores min y max.
        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(target.position.y, minHeight, maxHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        // Calcula cuánto se ha movido la cámara desde la última actualización.
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        // Mueve los fondos para crear un efecto de parallax.
        farBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

        // Mueve las partículas según si está lloviendo o no. Si es lluvia su eje y no varía, si son partículas normales si.
        if (itsRain)
        {
            particles.position += new Vector3(amountToMove.x, 0f, 0f);
        }
        else
        {
            particles.position += new Vector3(amountToMove.x, amountToMove.y, 0f);
        }

        // Actualiza la última posición de la cámara.
        lastPos = transform.position;
    }
}
