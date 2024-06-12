using System.Security.AccessControl;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.IO;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu instance;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public AudioMixerGroup sfxMixerGroup;
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> uniqueResolutions = new List<Resolution>(); // Lista de resoluciones únicas
    private int currentResolutionIndex;
    public GameObject panel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetMusicVolume();
        SetSFXVolume();
        LoadOptions();
        // Obtener todas las resoluciones disponibles
        Resolution[] allResolutions = Screen.resolutions;

        // Filtrar resoluciones únicas
        foreach (Resolution res in allResolutions)
        {
            bool alreadyExists = false;
            foreach (Resolution uniqueRes in uniqueResolutions)
            {
                if (uniqueRes.width == res.width && uniqueRes.height == res.height)
                {
                    alreadyExists = true;
                    break;
                }
            }
            if (!alreadyExists)
            {
                uniqueResolutions.Add(res);
            }
        }

        // Configurar las opciones del dropdown
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string option = uniqueResolutions[i].width + "x" + uniqueResolutions[i].height;
            options.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
    }

    // Cambiar la resolución elegida.
    public void OnResolutionChange()
    {
        Resolution resolution = uniqueResolutions[resolutionDropdown.value];

        // Si la resolución no es la que viene por defecto, se quita la pantalla completa.
        if (resolution.width == 1920 && resolution.height == 1080)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
            Screen.SetResolution(resolution.width, resolution.height, false);
        }
    }

    // Ajuste del volumen de la música.
    public void SetMusicVolume()
    {
        float mVolume = musicSlider.value;
        masterMixer.SetFloat("musicVolume", Mathf.Log10(mVolume) * 20);
    }

    // Ajuste del volumen de los efectos de sonido.
    public void SetSFXVolume()
    {
        float sVolume = sfxSlider.value;
        masterMixer.SetFloat("sfxVolume", Mathf.Log10(sVolume) * 20);
    }

    // Cerrar menú de opciones.
    public void CloseOptionsMenu()
    {
        panel.SetActive(false);
        Debug.Log("cerrao");
    }

    // Guardar configuración actual.
    public void SaveOptions()
    {
        OptionsData optionsData = new OptionsData();
        optionsData.musicVolume = musicSlider.value;
        optionsData.sfxVolume = sfxSlider.value;
        optionsData.resolutionIndex = resolutionDropdown.value;

        string jsonData = JsonUtility.ToJson(optionsData);
        File.WriteAllText(Application.persistentDataPath + "/options.json", jsonData);
    }

    // Cargar configuración guardada.
    public void LoadOptions()
    {
        string path = Application.persistentDataPath + "/options.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            OptionsData optionsData = JsonUtility.FromJson<OptionsData>(jsonData);

            musicSlider.value = optionsData.musicVolume;
            sfxSlider.value = optionsData.sfxVolume;
            resolutionDropdown.value = optionsData.resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }
}