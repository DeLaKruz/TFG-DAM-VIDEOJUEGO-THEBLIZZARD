using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime;

    // Sencillo script para eliminar de la escena un objeto con el tiempo que se le indique.
    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
