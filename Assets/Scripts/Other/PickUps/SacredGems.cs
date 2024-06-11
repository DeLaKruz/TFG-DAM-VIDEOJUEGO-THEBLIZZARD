using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// !DEJAR PARA EL ÚLTIMO POR COMENTAR PORQUE SE APLICAN CAMBIOS DURANTE TODO EL DESARROLLO
// !DEJAR PARA EL ÚLTIMO POR COMENTAR PORQUE SE APLICAN CAMBIOS DURANTE TODO EL DESARROLLO
// !DEJAR PARA EL ÚLTIMO POR COMENTAR PORQUE SE APLICAN CAMBIOS DURANTE TODO EL DESARROLLO
// !DEJAR PARA EL ÚLTIMO POR COMENTAR PORQUE SE APLICAN CAMBIOS DURANTE TODO EL DESARROLLO
public class SacredGems : MonoBehaviour
{
    public int gemType;
    public Texts texts;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gemType)
            {
                case 1:
                    PlayerController.instance.doubleJumpPower = true;
                    Destroy(gameObject);
                    playText();
                    break;
                case 2:
                    PlayerController.instance.dashpower = true;
                    PlayerController.instance.wallslidepower = true;
                    playText();
                    Destroy(gameObject);
                    break;
            }
        }
    }

    void playText()
    {
        DialogController[] dialogControllers = FindObjectsOfType<DialogController>();
        foreach (DialogController dialogController in dialogControllers)
        {
            dialogController.ActivateCartel(texts);
        }
    }
}
