using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Muerte : MonoBehaviour
{
    public Text textoMuerte;
    public static Muerte instance;

    void Awake()
    {
        instance = this;
    }

    public void activarMuerte()
    {
        UIController.instance.fadeToBlack();
        textoMuerte.gameObject.SetActive(true);
        PlayerController.instance.isDead = true;
        // Inicia la corrutina para realizar las acciones después de 3 segundos
        StartCoroutine(DoActionsAfterDelay(3.0f));
    }

    IEnumerator DoActionsAfterDelay(float delay)
    {
        // Espera durante el tiempo especificado (en segundos)
        yield return new WaitForSeconds(delay);

        // Ejecuta las acciones después del retraso
        UIController.instance.fadeFromBlack(true);
        textoMuerte.gameObject.SetActive(false);
        PlayerController.instance.isDead = false;
    }
}