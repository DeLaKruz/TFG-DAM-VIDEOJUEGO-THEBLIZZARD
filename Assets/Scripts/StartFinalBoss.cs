using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBoss : MonoBehaviour
{
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
