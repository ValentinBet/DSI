using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelsManager : MonoBehaviour
{
    [Header("Launch Button")]
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Game Infos")]
    [SerializeField] private TextMeshProUGUI remainingLifePointsText;

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
        buttonText.text = "Year " + (GameSettings.FIRST_YEAR + GameInfoManager.GameData.yearSurvived);
        remainingLifePointsText.text = GameInfoManager.GameData.yearSurvived + " / " + GameSettings.LIFE_POINTS;
        yearSurvivedText.text = GameInfoManager.GameData.yearSurvived > 1 ? GameInfoManager.GameData.yearSurvived + " Years survived" : GameInfoManager.GameData.yearSurvived + " Year survived";
    }
    public void UnloadLevelsScene()
    {
        SceneManager.UnloadSceneAsync("SelectLevels");
    }
}
