using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBoss : MonoBehaviour
{
    //Esta clase está hecha para un collider que se activa con el jefe final, se hizo para tener este texto como un caso aislado para futuro código del jefe.
    public Texts texts;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogController[] dialogControllers = FindObjectsOfType<DialogController>();
            foreach (DialogController dialogController in dialogControllers)
            {
                dialogController.ActivateCartel(texts);
            }
            DialogController.instance.finalboss = true;
            FinalBoss.instance.endBattleText = false;
        }
    }
}
