using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SettingsManager : MonoBehaviour
{
    public static readonly FullScreenMode[] Windowmodes = new FullScreenMode[4] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed };

    [Header("Video")]
    public TMP_Dropdown windowedDropdown;
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown qualityDropdown;
    public List<Vector2Int> ResolutionsList = new List<Vector2Int>();

    private bool resExist = false;
    private int resPlace;

    [Header("Audio")]
    public Slider generalVolumeSlider;
    public TMP_InputField generalVolumeInputField;

    [Header("Settings data")]
    public SettingsList settingsList;

    private SettingsSaves settingsSaves = new SettingsSaves();
    private string filename;


    private void Start()
    {
        filename = Application.persistentDataPath + "Settings" + ".json";
        InitSettingsList();

        InitResDropdown();
        InitQualityDropdown();

        InitVisuals();
    }

    public void InitVisuals()
    {
        InitVideoVisuals();
        InitAudioVisuals();
    }

    private void InitSettingsList()
    {
        settingsList.settings.Quality = QualitySettings.GetQualityLevel();
        settingsList.settings.Windowmode = (int)Screen.fullScreenMode;
    }

    private void InitVideoVisuals()
    {
        windowedDropdown.value = settingsList.settings.Windowmode;
        resDropdown.value = resPlace;
        qualityDropdown.value = settingsList.settings.Quality;
    }


    public void RevertChanges()
    {
        InitVisuals();
        MakeChanges();
    }

    public void ApplyChanges()
    {
        // Save settings >>
        settingsList.settings.Windowmode = windowedDropdown.value;

        resPlace = resDropdown.value;
        settingsList.settings.Resolution = resPlace;

        settingsList.settings.Quality = qualityDropdown.value;

        settingsList.settings.GeneralVolume = generalVolumeSlider.value;

        SaveAsJson();
        MakeChanges();
        InitSettingsList();
    }

    private void MakeChanges()
    {
        Screen.SetResolution(ResolutionsList[resDropdown.value].x, ResolutionsList[resDropdown.value].y, Windowmodes[settingsList.settings.Windowmode]);
        QualitySettings.SetQualityLevel(settingsList.settings.Quality);
        AudioListener.volume = generalVolumeSlider.value / 100;
    }
    public void SaveAsJson()
    {
        SettingsSaves settingsSaves = new SettingsSaves();

        settingsSaves.GeneralVolume = generalVolumeSlider.value / 100;
        string json = JsonUtility.ToJson(settingsSaves);
        File.WriteAllText(filename, json);
    }

    // <<

    // VIDEO SETTINGS >>

    private void InitResDropdown()
    {
        for (int x = 0; x < ResolutionsList.Count; x++)
        {
            resDropdown.options.Add(new TMP_Dropdown.OptionData() { text = ResolutionsList[x].x + " * " + ResolutionsList[x].y });

            if (ResolutionsList[x].x == Screen.width && ResolutionsList[x].y == Screen.height)
            {
                resExist = true;
                resPlace = x;
            }
        }

        if (!resExist)
        {
            resDropdown.options.Add(new TMP_Dropdown.OptionData() { text = Screen.width + " * " + Screen.height });
            ResolutionsList.Add(new Vector2Int(Screen.width, Screen.height));
            resPlace = resDropdown.options.Count;
        }

        resDropdown.RefreshShownValue();
    }

    private void InitQualityDropdown()
    {
        for (int x = 0; x < QualitySettings.names.Length; x++)
        {
            qualityDropdown.options.Add(new TMP_Dropdown.OptionData() { text = QualitySettings.names[x] });
        }

        qualityDropdown.RefreshShownValue();
    }

    // <<

    // AUDIO SETTINGS >>

    public void InitAudioVisuals()
    {
        using (StreamReader r = new StreamReader(filename))
        {
            var dataAsJson = r.ReadToEnd();
            settingsSaves = JsonUtility.FromJson<SettingsSaves>(dataAsJson);
        }

        generalVolumeInputField.text = (settingsSaves.GeneralVolume * 100).ToString();
        generalVolumeSlider.value = settingsSaves.GeneralVolume * 100;
    }

    public void OnGeneralVolumeSliderUpdate()
    {
        generalVolumeInputField.text = (generalVolumeSlider.value).ToString();
    }

    public void OnGeneralVolumeInputFieldUpdated()
    {
        if (float.Parse(generalVolumeInputField.text) > 100)
        {
            generalVolumeInputField.text = "100";
        }
        else if (float.Parse(generalVolumeInputField.text) < 0)
        {
            generalVolumeInputField.text = "0";
        }

        generalVolumeSlider.value = float.Parse(generalVolumeInputField.text);
    }

    // <<

    public void UnloadSettingsScene()
    {
        SceneManager.UnloadSceneAsync("Settings");
    }

}

public class KeyButtonParameters : MonoBehaviour
{
    public GameObject Button;
    public string keyName;
    public KeyCode Key;
}

[System.Serializable]
public class SettingsSaves
{
    public string AppVersion;
    public bool init;

    public float GeneralVolume;
}

[CreateAssetMenu(fileName = "SettingsList.asset", menuName = "Tools/Settings List", order = 100)]
public class SettingsList : ScriptableObject
{
    public Settings settings;
}

[System.Serializable]
public struct Settings
{
    public int Windowmode;
    public int Resolution;
    public int Quality;
    public float GeneralVolume;

    public float MouseSensitivity;
}
