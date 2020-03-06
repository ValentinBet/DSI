﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [HideInInspector] public bool isPlacingAlly = false;

    [Header("Waves Information")]
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Cluster Information")]
    [SerializeField] private GameObject cluster;
    [SerializeField] private Text clusterTitle;
    [SerializeField] private Text clusterDesc;
    [SerializeField] private Image clusterImg;
    private GameObject lastObjectOnCluster;

    [Header("Phase Button")]
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private GameObject nextTurnButton;

    [Header("AlertPanel")]
    [SerializeField] private Animator AlertAnim;
    [SerializeField] private TextMeshProUGUI AlertText;

    [Header("PAs")]
    [SerializeField] private Image[] PAdisplay;
    [SerializeField] private Sprite PAsprite;
    [SerializeField] private Sprite PAdisabled;
    //[SerializeField] private TextMeshProUGUI PAtext;

    [Header("Heroes")]
    [SerializeField] private Image[] heroSlots;
    [SerializeField] private GameObject lifeItem;
    private Image[,] lifeDisplays;

    [Header("Follow cursor image")]
    [SerializeField] private RectTransform allyHint;
    [SerializeField] private Image allyHintImg;

    private RaycastHit hit;
    private bool isAllyHintFollowingMouse = false;

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

    private void Update()
    {
        if (isAllyHintFollowingMouse)
        {
            allyHint.transform.position = Input.mousePosition;
        }
    }

    private void FixedUpdate()
    {
        if (isPlacingAlly)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.gameObject == lastObjectOnCluster)
            {
                return;
            }

            cluster.SetActive(true);

            lastObjectOnCluster = hit.collider.gameObject;

            switch (hit.collider.tag)
            {
                case "AllyCharacter":
                    AllyCharacter _ac = hit.collider.GetComponent<AllyCharacter>();
                    SetClusterInfo(_ac.data.name, _ac.data.allyDescription, _ac.ObjectTypeMetaData.icon);
                    break;
                case "EnemyCharacter":
                    EnemyCharacter _ec = hit.collider.GetComponent<EnemyCharacter>();
                    SetClusterInfo(_ec.name, _ec.enemyDescription, _ec.ObjectTypeMetaData.icon);
                    break;
                case "Tile":
                    TileProperties _tile = hit.collider.GetComponent<TileProperties>();
                    SetClusterInfo(_tile.tileName, _tile.tileDescription, _tile.ObjectTypeMetaData.icon);
                    break;
                default:
                    break;
            }
        }

    }

    public void SetAllyHintState(bool value, Sprite CharacterSprite = null)
    {
        allyHint.gameObject.SetActive(value);
        isAllyHintFollowingMouse = value;

        if (CharacterSprite != null)
        {
            SetAllyHintImg(CharacterSprite);
        }
    }

    public void SetAllyHintImg(Sprite CharacterSprite)
    {
        allyHintImg.sprite = CharacterSprite;
    }

    public void SetClusterInfo(string clusterTitle, string clusterDesc, Sprite clusterImg)
    {
        this.clusterTitle.text = clusterTitle;
        this.clusterDesc.text = clusterDesc;
        this.clusterImg.sprite = clusterImg;
    }

    public void EndTurnButtonClicked()
    {
        endTurnButton.GetComponent<Button>().interactable = false;
        PhaseManager.Instance.NextPhase(); // Logical transition to --> Ally Phase
    }

    public void newTurn()
    {
        endTurnButton.GetComponent<Button>().interactable = true;
        AlertAnim.Play("Alert");
        AlertText.text = "Player Phase";
    }

    public void allyTurn()
    {
        AlertAnim.Play("Alert");
        AlertText.text = "Allies phase";
    }

    public void enemyTurn()
    {
        AlertAnim.Play("Alert");
        AlertText.text = "Enemies phase";
    }

    public void setPA(int amount)
    {
        //PAtext.text = amount + "/5";
        for (int i = 0; i < PAdisplay.Length; i++)
        {
            if (i < amount)
            {
                PAdisplay[i].sprite = PAsprite;
            }
            else
            {
                PAdisplay[i].sprite = PAdisabled;
            }
        }
    }

    public void AllyLifeSetup()
    {
        lifeDisplays = new Image[3,5];
        for (int i = 0; i < CharactersManager.Instance.allyCharacter.Count;i++)
        {
            for (int j = 0; j < CharactersManager.Instance.allyCharacter[i].life; j++)
            {
                GameObject go = Instantiate(lifeItem, heroSlots[i].transform);
                go.GetComponent<RectTransform>().localPosition = -0.5f*Vector3.one;
                go.transform.localRotation = Quaternion.Euler(0.0f,0.0f,((-60.0f * (CharactersManager.Instance.allyCharacter[i].life - 1)) / 2.0f)+60.0f*j);
                lifeDisplays[i,j] = go.transform.GetChild(0).GetComponent<Image>();
            }
        }
    }

    public void AllyLifeUpdate(int allyID, int value)
    {
        for (int i = 0; i < heroSlots[allyID].transform.childCount; i++)
        {
            if (i < value)
            {
                lifeDisplays[allyID, i].color = Color.white;
            }
            else
            {
                lifeDisplays[allyID, i].color = Color.black;
            }
        }
    }
}
