﻿using System.Collections;
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
    [SerializeField] private List<Text> stateTextList = new List<Text>();
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite actualSprite;
    [SerializeField] private Sprite clearedSprite;

    [Header("Game Infos")]
    [SerializeField] private GameObject[] lifePointsElements;

    [Header("Chronology")]
    [SerializeField] private List<TextMeshProUGUI> chronologyTexts = new List<TextMeshProUGUI>();
    [SerializeField] private Image progressionBar;

    [Header("Quests")]
    [SerializeField] private GameObject questsLayout;
    [SerializeField] private GameObject archerAlert;


    private void Start()
    {
        InitVisuals();
    }

    private void InitVisuals()
    {
        if (GameInfoManager.GameData.yearSurvived - (3 - GameInfoManager.GameData.lifePoints) < 2)
        {
            archerAlert.SetActive(false);
        }
        for (int i = 0; i < yearButtonTextList.Count; i++)
        {
            yearButtonTextList[i].text = "Year " + (1 + i).ToString();//(GameSettings.FIRST_YEAR + GameInfoManager.GameData.yearSurvived) + i;
            chronologyTexts[i].text = (GameSettings.FIRST_YEAR+i).ToString();
            progressionBar.fillAmount = (float)(GameInfoManager.GameData.yearSurvived - (3 - GameInfoManager.GameData.lifePoints)) / 10.0f;
            if (i != GameInfoManager.GameData.yearSurvived - (3 - GameInfoManager.GameData.lifePoints))
            {
                if (i < GameInfoManager.GameData.yearSurvived - (3 - GameInfoManager.GameData.lifePoints))
                {
                    yearButtonTextList[i].GetComponentInParent<Image>().sprite = clearedSprite;
                    stateTextList[i].text = "Cleared !";
                }
                else
                {
                    yearButtonTextList[i].GetComponentInParent<Image>().sprite = lockedSprite;
                    stateTextList[i].text = "Locked...";
                }
                yearButtonTextList[i].transform.parent.localScale = Vector3.one * 0.7f;
                yearButtonTextList[i].GetComponentInParent<Button>().interactable = false;
            }
            else
            {
                yearButtonTextList[i].GetComponentInParent<Image>().sprite = actualSprite;
                stateTextList[i].text = "Click to Play !";
            }
        }

        for (int i =0; i < GameInfoManager.GameData.lifePoints && i < lifePointsElements.Length;i++)
        {
            lifePointsElements[i].SetActive(false);
        }
    }

    public void LaunchGame(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }

    public void UnlockAllLevels()
    {
        for (int i = 0; i < yearButtonTextList.Count; i++)
        {
            yearButtonTextList[i].transform.parent.localScale = Vector3.one;
            yearButtonTextList[i].GetComponentInParent<Button>().interactable = true;
            yearButtonTextList[i].GetComponentInParent<Image>().sprite = actualSprite;
            stateTextList[i].text = "Click to Play !";
        }
    }

    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadLevelsScene()
    {
        SceneManager.UnloadSceneAsync("SelectLevels");
    }
}
