using System.Collections;
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
    [SerializeField] private Animator clusterAnim;
    [SerializeField] private Text clusterTitle;
    [SerializeField] private Text clusterDesc;
    [SerializeField] private Text clusterLife;

    private int animCharge;
    private GameObject lastObjectOnCluster;

    [Header("Phase Button")]
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private GameObject nextTurnButton;

    [Header("AlertPanel")]
    [SerializeField] private Animator AlertAnim;
    [SerializeField] private Image alertImage;
    [SerializeField] private Sprite alertPlayer;
    [SerializeField] private Sprite alertAlly;
    [SerializeField] private Sprite alertEnemy;

    [Header("PAs")]
    [SerializeField] private Image[] PAdisplay;
    [SerializeField] private Sprite PAsprite;
    [SerializeField] private Sprite PAdisabled;
    //[SerializeField] private TextMeshProUGUI PAtext;
    [SerializeField] private GameObject tileMovementObj;

    [Header("Heroes")]
    [SerializeField] private Image[] heroSlots;
    [SerializeField] private List<TextMeshPro> allyTextDisplay = new List<TextMeshPro>();
    [SerializeField] private GameObject lifeItem;
    private Image[,] lifeDisplays;

    [Header("LotusLife")]
    [SerializeField] private GameObject[] lotusLife;

    [Header("Follow cursor image")]
    [SerializeField] private RectTransform mouseFollowObj;
    [SerializeField] private GameObject rotateHint;
    [SerializeField] private GameObject swapHint;

    [Header("Keyboard Help")]
    [SerializeField] private GameObject cancelHelpKey;
    [SerializeField] private GameObject quitModeHelpKey;

    [Header("Highlight")]
    [SerializeField] private GameObject rotateHighlight;
    [SerializeField] private GameObject swapHighlight;

    [Header("General")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject endScreenVictory;
    [SerializeField] private GameObject endScreenDefeat;

    private RaycastHit hit;
    private bool ObjFollowingMouse = false;

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
        if (ObjFollowingMouse)
        {
            mouseFollowObj.transform.position = Input.mousePosition;
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

            if (animCharge < 0) animCharge = 0;
            ClusterAnimCharge(1);
            lastObjectOnCluster = hit.collider.gameObject;

            switch (hit.collider.tag)
            {
                case "AllyCharacter":
                    AllyCharacter _ac = hit.collider.GetComponent<AllyCharacter>();
                    SetClusterInfo(_ac.allyName, _ac.allyDescription, _ac.life, _ac.maxLife);
                    break;
                case "EnemyCharacter":
                    EnemyCharacter _ec = hit.collider.GetComponent<EnemyCharacter>();
                    SetClusterInfo(_ec.name, _ec.enemyDescription, _ec.life, _ec.maxLife);
                    break;
                case "Tile":
                    TileProperties _tile = hit.collider.GetComponent<TileProperties>();
                    SetClusterInfo(_tile.tileName, _tile.tileDescription, null, null);
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (animCharge > 0) animCharge = 0;
            ClusterAnimCharge(-1);
        }
    }

    private void ClusterAnimCharge(int value)
    {
        animCharge += value;
        if(animCharge == 1)
        {

            clusterAnim.SetTrigger("Activate");
        }
        else if (animCharge == -5)
        {

            clusterAnim.SetTrigger("Fold");
        }
    }

    public void InitEndGame(bool isVictory)
    {
        menu.SetActive(false);
        endScreen.SetActive(true);

        if (isVictory)
        {
            endScreenVictory.SetActive(true);
        } else
        {
            endScreenDefeat.SetActive(false);
        }
    }

    public void SetClusterInfo(string clusterTitle, string clusterDesc, int? actualLife = null, int? maxLife = null) //int null (vie non obligatoire)
    {
        this.clusterTitle.text = clusterTitle;
        this.clusterDesc.text = clusterDesc;

        if (!(actualLife == null) || !(maxLife == null))
        {
            this.clusterLife.text = actualLife + "/" + maxLife;
        } else
        {
            this.clusterLife.text = "";
        }

    }

    public void EndTurnButtonClicked()
    {
        endTurnButton.GetComponent<Button>().interactable = false;
        PhaseManager.Instance.NextPhase(); // Logical transition to --> Ally Phase
    }

    public void NewTurn()
    {
        endTurnButton.GetComponent<Button>().interactable = true;
        AlertAnim.Play("Alert");
        alertImage.sprite = alertPlayer;
    }

    public void AllyTurn()
    {
        AlertAnim.Play("Alert");
        alertImage.sprite = alertAlly;
    }

    public void EnemyTurn()
    {
        AlertAnim.Play("Alert");
        alertImage.sprite = alertEnemy;
    }

    public void SetPA(int amount)
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
        lifeDisplays = new Image[3, 5];
        for (int i = 0; i < CharactersManager.Instance.allyCharacter.Count; i++)
        {
            for (int j = 0; j < CharactersManager.Instance.allyCharacter[i].life; j++)
            {
                GameObject go = Instantiate(lifeItem, heroSlots[i].transform); // Change to pooling
                go.GetComponent<RectTransform>().localPosition = new Vector3(-1f, -1.1f, -0.1f);
                go.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, ((40.0f * (CharactersManager.Instance.allyCharacter[i].life - 1)) / 2.0f) - 40.0f * j);
                lifeDisplays[i, j] = go.transform.GetChild(0).GetComponent<Image>();
            }
        }
    }

    public void BaseLifeUpdate(int value)
    {
        for (int i = 0; i < lotusLife.Length; i++)
        {
            if (i < value && !lotusLife[i].activeSelf)
            {
                lotusLife[i].SetActive(true);
            }
            else
            {
                lotusLife[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Updates player life
    /// </summary>
    /// <param name="allyID">this character ID (his priority)</param>
    /// <param name="value">this character new life value</param>
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

    public void SetAllyLevelDisplay(int value)
    {
        allyTextDisplay[value].text = "LVL " + CharactersManager.Instance.allyCharacter[value].level;
    }

    private void LaunchMode(bool value)
    {
        EndMode();
        quitModeHelpKey.SetActive(value);
        ObjFollowingMouse = value;
        AudioManager.Instance.PlayButtonClick();
    }

    public void DisplayRotate(bool value)
    {
        LaunchMode(value);
        rotateHint.SetActive(value);
    }

    public void DisplaySwap(bool value)
    {
        LaunchMode(value);
        swapHint.SetActive(value);
    }

    public void EndMode()
    {
        quitModeHelpKey.SetActive(false);
        cancelHelpKey.SetActive(false);
        ObjFollowingMouse = false;
        swapHint.SetActive(false);
        rotateHint.SetActive(false);
        StopHighlight();
    }

    public void DisplayCancelHelpKey(bool value)
    {
        cancelHelpKey.SetActive(value);
    }


    public void SetTileMovementObj(bool value)
    {
        tileMovementObj.SetActive(value);
    }

    public void UpdateWave(int actualWave, int waveAmount)
    {
        waveText.text = "Wave " + actualWave + '/' + waveAmount;
    }

    public void HighlightSwap()
    {
        rotateHighlight.SetActive(false);
        swapHighlight.SetActive(true);
    }

    public void HighlightRotate()
    {
        rotateHighlight.SetActive(true);
        swapHighlight.SetActive(false);
    }

    public void StopHighlight()
    {
        rotateHighlight.SetActive(false);
        swapHighlight.SetActive(false);
    }

}
