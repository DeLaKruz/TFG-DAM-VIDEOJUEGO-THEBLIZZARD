using System.Security.AccessControl;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Clase para definir los datos que se van a guardar.
public class DataGame
{
    public UnityEngine.Vector3 position;
    public int life;
    public int money;
    public bool doubleJump;
    //dash, wallslide;
    public int currentScene;
}
