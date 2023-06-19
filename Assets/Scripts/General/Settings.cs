using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using Sirenix.OdinInspector;
using TMPro;

[System.Serializable]
public class SettingsData
{
    public int fullscreen;
    [Space(10)]
    public int resolutionIndex;
    [Space(10)]
    public float masterVolume;
    [Space(10)]
    public float musicVolume;
    [Space(10)]
    public float soundEffectsVolume;
    [Space(10)]
    public float xSensitivity;
    [Space(10)]
    public float ySensitivity;
}
public class Settings : MonoBehaviour
{
    public string saveFile;
    public SettingsData settingsData = new SettingsData();
    public PlayerController playerController;
    [BoxGroup("Fullscreen")]
    public TMP_Dropdown fullscreenDropdown;
    
    [BoxGroup("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    [BoxGroup("Resolution")]
    public Resolution[] resolutions;
    [BoxGroup("Resolution")]
    public List<string> reso;

    [BoxGroup("Volume")]
    public AudioMixer audioMixer;
    [BoxGroup("Volume")]
    public Slider masterVolSlider;
    [BoxGroup("Volume")]
    public Slider musicVolSlider;
    [BoxGroup("Volume")]
    public Slider soundEffectsSlider;
    [BoxGroup("Volume")]
    public TMP_Text masterVolText;
    [BoxGroup("Volume")]
    public TMP_Text musicVolText;
    [BoxGroup("Volume")]
    public TMP_Text soundEffectsVolText;

    [BoxGroup("Sensitivity")]
    public Slider xSensitivitySlider;
    [BoxGroup("Sensitivity")]
    public Slider ySensitivitySlider;
    [BoxGroup("Sensitivity")]
    public TMP_Text xSensitivityText;
    [BoxGroup("Sensitivity")]
    public TMP_Text ySensitivityText;

        
    void Awake()
    {
        saveFile = PlayerPrefs.GetString("Most Recent Save");
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            reso.Add( resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz");
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        
        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {
        saveFile = PlayerPrefs.GetString("Most Recent Save");
        switch (fullscreenDropdown.value)
        {
            case 0: 
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            break;  

            case 1:             
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            break; 

            case 2: 
                Screen.fullScreenMode = FullScreenMode.Windowed;
            break;           
        }    
        #region audio
        audioMixer.SetFloat("MasterVolume", masterVolSlider.value);
        audioMixer.SetFloat("MusicVolume", musicVolSlider.value);
        audioMixer.SetFloat("EffectsVolume", soundEffectsSlider.value);

        masterVolText.text = "Master Volume (" + (masterVolSlider.value + 80) + "): ";
        musicVolText.text = "Music Volume (" + (musicVolSlider.value + 80) + "): ";
        soundEffectsVolText.text = "Voices Volume (" + (soundEffectsSlider.value + 80) + "): ";
        #endregion
        
        /*if(PlayerManager.instance){
            PlayerManager.instance.xSensitivity = xSensitivitySlider.value * 1.5f;
            PlayerManager.instance.ySensitivity = ySensitivitySlider.value * 1.5f;
        }*/

        xSensitivityText.text = "X Sensitivity (" + xSensitivitySlider.value + "):";
        ySensitivityText.text = "Y Sensitivity (" + ySensitivitySlider.value + "):";
        playerController.mouseSensitivityX = xSensitivitySlider.value / 10;
        playerController.mouseSensitivityY = ySensitivitySlider.value / 10;
    }

    public void Back()
    {
        WriteFile();
    }
     
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    [ContextMenu("Load")]
    public void ReadFile()
    {
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            
            settingsData = JsonUtility.FromJson<SettingsData>(fileContents);
            
            fullscreenDropdown.value = settingsData.fullscreen;
            resolutionDropdown.value = settingsData.resolutionIndex;

            masterVolSlider.value = settingsData.masterVolume;
            musicVolSlider.value = settingsData.musicVolume;
            soundEffectsSlider.value = settingsData.soundEffectsVolume;

            xSensitivitySlider.value = settingsData.xSensitivity;
            ySensitivitySlider.value = settingsData.ySensitivity;
        
        }
    }


    [ContextMenu("Save")]
    public void WriteFile()
    {
        settingsData.fullscreen = fullscreenDropdown.value;
        settingsData.resolutionIndex = resolutionDropdown.value;

        settingsData.masterVolume = masterVolSlider.value;
        settingsData.musicVolume = musicVolSlider.value;
        settingsData.soundEffectsVolume = soundEffectsSlider.value;

        settingsData.xSensitivity = xSensitivitySlider.value;
        settingsData.ySensitivity = ySensitivitySlider.value;

        
        saveFile = Application.persistentDataPath + "/" + "Settings" + ".json";
        Debug.Log(saveFile);
        PlayerPrefs.SetString("Most Recent Save", saveFile);

        string jsonString = JsonUtility.ToJson(settingsData, true);

        File.WriteAllText(saveFile, jsonString);
        
    }
}
