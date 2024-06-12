using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnterOtherMap : MonoBehaviour
{
    public List<GameObject> spriteObjectsToMakeTransparent; // Lista de objetos con SpriteRenderer que se volverán transparentes
    public List<GameObject> tilemapObjectsToMakeTransparent; // Lista de objetos con TilemapRenderer que se volverán transparentes
    public float transparencyLevel = 0.5f; // Nivel de transparencia objetivo
    public float transitionDuration = 1.0f; // Duración de la transición en segundos

    // Método llamado cuando un objeto entra en el trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Inicia la transición para hacer los objetos transparentes
            StartCoroutine(ChangeTransparency(transparencyLevel, transitionDuration));
        }
    }

    // Método llamado cuando un objeto sale del trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Inicia la transición para hacer los objetos opacos nuevamente
            StartCoroutine(ChangeTransparency(1.0f, transitionDuration)); // Establecer a completamente opaco
        }
    }

    // Corrutina que cambia la transparencia de los objetos gradualmente
    private IEnumerator ChangeTransparency(float targetAlpha, float duration)
    {
        float startAlpha = GetCurrentAlpha(); // Obtiene la transparencia inicial
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // Interpola entre la transparencia inicial y la transparencia objetivo
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            // Establece la nueva transparencia
            SetTransparency(newAlpha);
            yield return null;
        }

        // Asegura que se establece el valor final de transparencia
        SetTransparency(targetAlpha);
    }

    // Obtiene la transparencia actual del primer objeto en las listas
    private float GetCurrentAlpha()
    {
        if (spriteObjectsToMakeTransparent.Count > 0)
        {
            SpriteRenderer spriteRenderer = spriteObjectsToMakeTransparent[0].GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                return spriteRenderer.material.color.a;
            }
        }

        if (tilemapObjectsToMakeTransparent.Count > 0)
        {
            TilemapRenderer tilemapRenderer = tilemapObjectsToMakeTransparent[0].GetComponent<TilemapRenderer>();
            if (tilemapRenderer != null)
            {
                return tilemapRenderer.material.color.a;
            }
        }

        return 1.0f; // Por defecto, completamente opaco si no se encuentran objetos
    }

    // Establece la transparencia de todos los objetos en las listas
    private void SetTransparency(float alpha)
    {
        // Ajusta la transparencia para objetos con SpriteRenderer
        foreach (GameObject obj in spriteObjectsToMakeTransparent)
        {
            if (obj != null)
            {
                SetSpriteTransparency(obj, alpha);
            }
        }

        // Ajusta la transparencia para objetos con TilemapRenderer
        foreach (GameObject obj in tilemapObjectsToMakeTransparent)
        {
            if (obj != null)
            {
                SetTilemapTransparency(obj, alpha);
            }
        }
    }

    // Establece la transparencia de los SpriteRenderers en el objeto dado
    private void SetSpriteTransparency(GameObject obj, float alpha)
    {
        SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                Material material = spriteRenderer.material;
                if (material != null)
                {
                    Color color = material.color;
                    color.a = alpha;
                    material.color = color;
                }
            }
        }
    }

    // Establece la transparencia del TilemapRenderer en el objeto dado
    private void SetTilemapTransparency(GameObject obj, float alpha)
    {
        TilemapRenderer tilemapRenderer = obj.GetComponent<TilemapRenderer>();
        if (tilemapRenderer != null)
        {
            Material material = tilemapRenderer.material;
            if (material != null)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }
}