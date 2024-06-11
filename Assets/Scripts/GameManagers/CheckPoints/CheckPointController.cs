using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public static CheckPointController instance;
    public CheckPointBFS[] checkPoints; //Lista de checkpoints.
    public UnityEngine.Vector3 spawnPoint;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Encuentra todos los objetos de tipo CheckPointBFS y establece el primer spawnpoint donde aparece el jugador.
        checkPoints = FindObjectsOfType<CheckPointBFS>();
        spawnPoint = PlayerController.instance.transform.position;
    }

    //Establece el nuevo spawnpoint con el vector que le pasamos al llamar al método.
    public void SetSpawnPoint(UnityEngine.Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
