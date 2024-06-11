using UnityEngine;
using UnityEngine.SceneManagement;

// Clase para controlar la lógica de un cofre en el juego.
public class ChestLogic : MonoBehaviour
{
    public int chestID; // Identificador único del cofre.
    private Animator anim; // Referencia al componente Animator del cofre.
    private Collider2D chestCollider; // Referencia al collider del cofre.
    public bool isOpened = false; // Indica si el cofre está abierto o no.

    // Variable estática para controlar si se debe guardar el estado de los cofres.
    private static bool guardarEstadoCofres = false;

    void Start()
    {
        // Obtener referencias a los componentes Animator y Collider2D.
        anim = GetComponent<Animator>();
        chestCollider = GetComponent<Collider2D>();

        // Generar una clave única para el cofre basada en el nombre de la escena y su identificador.
        string chestKey = GetChestKey();

        // Comprobar si el cofre ya ha sido abierto en partidas anteriores.
        if (PlayerPrefs.HasKey(chestKey))
        {
            isOpened = PlayerPrefs.GetInt(chestKey) == 1;
            if (isOpened)
            {
                chestCollider.enabled = false; // Desactivar el collider si el cofre está abierto.
            }
        }
        anim.SetBool("isOpened", false); // Establecer la animación de abrir cofre a falso.
    }

    void Update()
    {
        // Si se activa la variable para guardar el estado de los cofres y este cofre está abierto guardar su estado.
        if (guardarEstadoCofres && isOpened)
        {
            string chestKey = GetChestKey();
            PlayerPrefs.SetInt(chestKey, isOpened ? 1 : 0); // Guardar el estado del cofre en PlayerPrefs.
            PlayerPrefs.Save(); // Guardar los cambios en PlayerPrefs.
        }

        // DEBUG: Reiniciar el estado de todos los cofres si se presiona la tecla 'M'.
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChestLogic.CambiarEstadoGuardarCofres(true); // Cambiar el estado para guardar los cofres.
            ReiniciarValoresPlayerPrefs(); // Reiniciar los valores en PlayerPrefs.
        }
    }

    // Método para abrir el cofre.
    public void AbrirCofre()
    {
        if (isOpened)
            return;
        isOpened = true; // Establecer el cofre como abierto.
        anim.SetBool("isOpened", true); // Activar la animación de abrir cofre.
        Invoke("EstablecerPermanentlyOpened", 0.4f); // Invocar el método para establecer el estado permanentemente abierto.

        chestCollider.enabled = false; // Desactivar el collider del cofre.
    }

    // Método para establecer el estado del cofre como permanentemente abierto.
    void EstablecerPermanentlyOpened()
    {
        anim.SetBool("permanentlyOpened", true);
    }

    // Método para obtener la clave única del cofre en PlayerPrefs.
    private string GetChestKey()
    {
        return "Chest_" + SceneManager.GetActiveScene().name + "_" + chestID;
    }

    // Método para reiniciar los valores de PlayerPrefs de todos los cofres.
    public void ReiniciarValoresPlayerPrefs()
    {
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            Scene scene = SceneManager.GetSceneAt(sceneIndex);
            GameObject[] rootObjects = scene.GetRootGameObjects();

            foreach (GameObject rootObject in rootObjects)
            {
                ChestLogic[] chestLogics = rootObject.GetComponentsInChildren<ChestLogic>();

                foreach (ChestLogic chestLogic in chestLogics)
                {
                    string chestKey = chestLogic.GetChestKey();
                    PlayerPrefs.DeleteKey(chestKey); // Eliminar la clave del cofre en PlayerPrefs.
                    chestLogic.isOpened = false; // Establecer el cofre como cerrado.
                    chestLogic.anim.SetBool("isOpened", false); // Desactivar la animación de abrir cofre.
                    chestLogic.anim.SetBool("permanentlyOpened", false); // Desactivar el estado permanentemente abierto.
                }
            }
        }
    }

    // Método para comprobar si el cofre está abierto permanentemente.
    public void ComprobarAbiertoPermanentemente()
    {
        // Comprueba si el cofre está abierto y si cumple ciertas condiciones para ser permanente.
        if (isOpened)
        {
            // Establece el estado del cofre como permanentemente abierto.
            anim.SetBool("permanentlyOpened", true);
        }
    }

    // Método para cambiar el estado de guardar el estado de los cofres.
    public static void CambiarEstadoGuardarCofres(bool estado)
    {
        // Accede al campo estático 'guardarEstadoCofres' de la clase 'ChestLogic' y modifícalo con el valor proporcionado.
        ChestLogic.guardarEstadoCofres = estado;
    }
}