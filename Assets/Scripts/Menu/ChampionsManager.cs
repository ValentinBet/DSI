using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChampionsManager : MonoBehaviour
{
  //  [SerializeField] private List<GameObject> TabElement = new List<GameObject>();
    [SerializeField] private Color activatedTabElementColor;
    [SerializeField] private Color desactivatedTabElementColor;

    [SerializeField] private List<TextMeshProUGUI> nameListText = new List<TextMeshProUGUI>();

    [SerializeField] private Sprite[] allySprite = null;
    [SerializeField] private Image allyImage = null;

    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI levelText = null;
    [SerializeField] private TextMeshProUGUI lifePointsText = null;
    [SerializeField] private Text descriptionText = null;
    [SerializeField] private TextMeshProUGUI yearSurvivedText = null;
    [SerializeField] private TextMeshProUGUI enemyKilledText = null;

    [SerializeField] private GameObject championTab = null;
    [SerializeField] private GameObject TabElements = null;


    private void Start()
    {
        // ActivateTabElement(0);
        Init();
    }

    //private void ResetTabElementColor()
    //{
    //    foreach (GameObject TbElmt in TabElement)
    //    {
    //        TbElmt.GetComponent<Image>().color = desactivatedTabElementColor;
    //    }
    //}

    private void Init()
    {
        for (int i = 0; i < nameListText.Count; i++)
        {
            nameListText[i].text = GameInfoManager.GameData.allies[i].name;
        }
    }
    public void ActivateTabElement(int value)
    {
        //ResetTabElementColor();
        //TabElement[value].GetComponent<Image>().color = activatedTabElementColor;

        UpdateTab(value);
        championTab.SetActive(true);
        TabElements.SetActive(false);
    }

    public void QuitTabElement()
    {
        championTab.SetActive(false);
        TabElements.SetActive(true);
    }


    private void UpdateTab(int value)
    {
        AllyCharacterSave _ac = GameInfoManager.GameData.allies[value];
        // allySprite.sprite = _ac.ObjectTypeMetaData.sprite;


        allyImage.sprite = allySprite[(int)_ac.type];
        nameText.text = _ac.name;
        levelText.text = /*"LEVEL : " + */_ac.level.ToString();
        lifePointsText.text = "LIFE POINTS : " +_ac.life;
        descriptionText.text = "DESCRIPTION : \n" + _ac.allyDescription;
        yearSurvivedText.text = "YEAR SURVIVED : " + _ac.yearSurvived;
        enemyKilledText.text = "ENEMY KILLED : " + _ac.enemyKilled;
    }

    public void UnloadChampionsScene()
    {
        SceneManager.UnloadSceneAsync("Champions");
    }  


}
