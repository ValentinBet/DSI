﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChampionsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> TabElement = new List<GameObject>();
    [SerializeField] private Color activatedTabElementColor;
    [SerializeField] private Color desactivatedTabElementColor;

    [SerializeField] private Image allySprite;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI lifePointsText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private GameObject patternLayout;
    [SerializeField] private GameObject amuletsLayout;


    private void Start()
    {
        ActivateTabElement(0);
    }

    private void ResetTabElementColor()
    {
        foreach (GameObject TbElmt in TabElement)
        {
            TbElmt.GetComponent<Image>().color = desactivatedTabElementColor;
        }
    }

    public void ActivateTabElement(int value)
    {
        ResetTabElementColor();
        TabElement[value].GetComponent<Image>().color = activatedTabElementColor;

        UpdateTab(value);
    }

    private void UpdateTab(int value)
    {
        AllyCharacterSave _ac = GameInfoManager.GameData.allies[value];
        // allySprite.sprite = _ac.ObjectTypeMetaData.sprite;
        nameText.text = _ac.name;
        levelText.text = "Level : " + _ac.level;
        lifePointsText.text = "Life points : " +_ac.life;
        descriptionText.text = "Description : \n" + _ac.allyDescription;
    }

    public void UnloadChampionsScene()
    {
        SceneManager.UnloadSceneAsync("Champions");
    }


}
