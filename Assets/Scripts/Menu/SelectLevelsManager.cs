using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelsManager : MonoBehaviour
{
    public string LevelScene;

    [Header("Launch Button")]
    [SerializeField] private List<TextMeshProUGUI> yearButtonTextList = new List<TextMeshProUGUI>();

    [Header("Game Infos")]
    [SerializeField] private GameObject[] lifePointsElements;

    [Header("Years survived panel")]
    [SerializeField] private TextMeshProUGUI yearSurvivedText;

    [Header("Quests")]
    [SerializeField] private GameObject questsLayout;

    private void Start()
    {
        InitVisuals();
    }

    private void InitVisuals()
    {
        for (int i = 0; i < yearButtonTextList.Count; i++)
        {
            yearButtonTextList[i].text = "Year " + (GameSettings.FIRST_YEAR + GameInfoManager.GameData.yearSurvived) + i;
        }

        for (int i =0; i < GameInfoManager.GameData.lifePoints && i < lifePointsElements.Length;i++)
        {
            lifePointsElements[i].SetActive(false);
        }
        yearSurvivedText.text = GameInfoManager.GameData.yearSurvived > 1 ? GameInfoManager.GameData.yearSurvived + " Years survived" : GameInfoManager.GameData.yearSurvived + " Year survived";
    }

    public void LaunchGame(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }

    public void UnloadLevelsScene()
    {
        SceneManager.UnloadSceneAsync("SelectLevels");
    }
}
