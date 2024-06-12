using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Permite que la clase sea serializable y visible en el Inspector de Unity
public class Texts
{
    [TextArea(2, 6)] // Hace que el campo textsArray sea un campo de texto en el Inspector de Unity, con un tamaño mínimo de 2 líneas y máximo de 6 líneas
    public string[] textsArray; // Array que almacena las líneas de texto del diálogo
}