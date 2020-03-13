using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }

    [SerializeField] private TextMeshProUGUI selectLevelsText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        InitVisuals();
    }

    private void InitVisuals()
    {
        selectLevelsText.text = GameInfoManager.GameData.Init ? "PLAY" : "NEW GAME";
    }

    public void InitLevelScene()
    {
        LoadSceneAdditive("SelectLevels");
        GameInfoManager.GameData.Init = true;
        GameInfoManager.SaveGameDataAsJson();
    }

    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
