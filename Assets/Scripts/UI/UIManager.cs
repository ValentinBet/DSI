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
    [SerializeField] private TextMeshProUGUI waveText =null;

    [Header("Cluster Information")]
    [SerializeField] private Animator clusterAnim = null;
    [SerializeField] private Text clusterTitle = null;
    [SerializeField] private Text clusterDesc = null;
    [SerializeField] private Text clusterLife = null;

    private int animCharge = 0;
    private GameObject lastObjectOnCluster = null;

    [Header("Phase Button")]
    [SerializeField] private GameObject endTurnButton = null;

    [Header("AlertPanel")]
    [SerializeField] private Animator AlertAnim = null;
    [SerializeField] private Image alertImage =null;
    [SerializeField] private Sprite alertPlayer =null;
    [SerializeField] private Sprite alertAlly = null;
    [SerializeField] private Sprite alertEnemy = null;

    [Header("PAs")]
    public Image[] PAdisplay = null;
    [SerializeField] private Sprite PAsprite = null;
    [SerializeField] private Sprite PAdisabled = null;
    //[SerializeField] private TextMeshProUGUI PAtext;
    [SerializeField] private GameObject tileMovementObj = null;

    [Header("Heroes")]
    [SerializeField] private Image[] heroSlots = null;
    [SerializeField] private List<TextMeshPro> allyTextDisplay = new List<TextMeshPro>();
    [SerializeField] private GameObject lifeItem = null;
    private Image[,] lifeDisplays = null;

    [Header("LotusLife")]
    [SerializeField] private GameObject[] lotusLife = null;
    [SerializeField] private GameObject[] lotusLifeVFX = null;

    [Header("Follow cursor image")]
    [SerializeField] private RectTransform mouseFollowObj = null;
    [SerializeField] private GameObject rotateHint = null;
    [SerializeField] private GameObject swapHint = null;

    [Header("Keyboard Help")]
    [SerializeField] private GameObject cancelHelpKey = null;
    [SerializeField] private GameObject quitModeHelpKey = null;

    [Header("Highlight")]
    [SerializeField] private GameObject rotateHighlight = null;
    [SerializeField] private GameObject swapHighlight = null;

    [Header("General")]
    [SerializeField] private GameObject menu = null;
    [SerializeField] private GameObject endScreen = null;
    [SerializeField] private GameObject endScreenVictory = null;
    [SerializeField] private GameObject endScreenDefeat = null;

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

    /// <summary>
    /// Orienter l'animation du cluster vers l'ouverture ou la fermeture (utilisé pour placer un délai sur les animations du cluster)
    /// </summary>
    /// <param name="value">valeur à appliquer pour la modification de l'animation</param>
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

    /// <summary>
    /// Fonction mettant en place l'interface de fin de partie
    /// </summary>
    /// <param name="isVictory">true : victoire / false : défaite</param>
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

    /// <summary>
    /// Définis les informations à écrire dans le cluster
    /// </summary>
    /// <param name="clusterTitle">titre du cluster</param>
    /// <param name="clusterDesc">description du cluster</param>
    /// <param name="actualLife">vie actuelle (laisser null si ce n'est pas un personnage)</param>
    /// <param name="maxLife">vie maximale (laisser null si ce n'est pas un personnage)</param>
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

    /// <summary>
    /// Fonction communiquant la fin du tour du joueur à l'interface
    /// </summary>
    public void EndTurnButtonClicked()
    {
        endTurnButton.GetComponent<Button>().interactable = false;
        PhaseManager.Instance.NextPhase(); // Logical transition to --> Ally Phase
    }

    /// <summary>
    /// Fonction communiquant le début du tour du joueur à l'interface
    /// </summary>
    public void NewTurn()
    {
        endTurnButton.GetComponent<Button>().interactable = true;
        AlertAnim.Play("Alert");
        alertImage.sprite = alertPlayer;
    }

    /// <summary>
    /// Fonction communiquant le début du tour des alliés à l'interface
    /// </summary>
    public void AllyTurn()
    {
        AlertAnim.Play("Alert");
        alertImage.sprite = alertAlly;
    }

    /// <summary>
    /// Fonction communiquant le début du tour des ennemis à l'interface
    /// </summary>
    public void EnemyTurn()
    {
        AlertAnim.Play("Alert");
        alertImage.sprite = alertEnemy;
    }

    /// <summary>
    /// Fonction définissant le nombre de PA à afficher
    /// </summary>
    /// <param name="amount">nouveau montant de PA</param>
    public void SetPA(int amount)
    {
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

    /// <summary>
    /// Initialisation de la barre de vie des personnages
    /// </summary>
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

    /// <summary>
    /// Mise à jour de la vie de la base (nombre de pétales sur le lotus)
    /// </summary>
    /// <param name="value">Nouvelle valeur de vie</param>
    public void BaseLifeUpdate(int value)
    {
        for (int i = 0; i < lotusLife.Length; i++)
        {
            if (i < value)
            {
                if (!lotusLife[i].activeSelf)
                    lotusLife[i].SetActive(true);
            }
            else
            {
                lotusLife[i].SetActive(false);
           
            }
        }

        for (int i = 0; i < lotusLifeVFX.Length; i++)
        {
            if (i > value && lotusLifeVFX[i].activeSelf)
            {
                lotusLifeVFX[i].SetActive(true);
            }
        }

    }

    /// <summary>
    /// Mets à jour la vie du joueur
    /// </summary>
    /// <param name="allyID">L'ID de ce personnage (sa priorité)</param>
    /// <param name="value">La nouvelle valeur de vie de ce personnage</param>
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

    /// <summary>
    /// Mets à jour le niveau d'expérience d'un personnage
    /// </summary>
    /// <param name="ID">index du personnage (sa priorité)</param>
    public void SetAllyLevelDisplay(int ID)
    {
        allyTextDisplay[ID].text = "LVL " + CharactersManager.Instance.allyCharacter[ID].level;
    }

    /// <summary>
    /// Update du mode d'utilisation du controller
    /// </summary>
    /// <param name="value">est-ce que le mode est actif ?</param>
    private void LaunchMode(bool value)
    {
        EndMode();
        quitModeHelpKey.SetActive(value);
        ObjFollowingMouse = value;
        AudioManager.Instance.PlayButtonClick();
    }

    /// <summary>
    /// affiche ou non le mode "Rotation"
    /// </summary>
    /// <param name="value">true : début/ false : fin du mode</param>
    public void DisplayRotate(bool value)
    {
        LaunchMode(value);
        rotateHint.SetActive(value);
    }

    /// <summary>
    /// affiche ou non le mode "Swap"
    /// </summary>
    /// <param name="value">true : début/ false : fin du mode</param>
    public void DisplaySwap(bool value)
    {
        LaunchMode(value);
        swapHint.SetActive(value);
    }

    /// <summary>
    /// Fin du mode, activé à la fin d'un tour du joueur
    /// </summary>
    public void EndMode()
    {
        quitModeHelpKey.SetActive(false);
        cancelHelpKey.SetActive(false);
        ObjFollowingMouse = false;
        swapHint.SetActive(false);
        rotateHint.SetActive(false);
        StopHighlight();
    }

    /// <summary>
    /// Afficher ou non la touche d'aide "Cancel"
    /// </summary>
    /// <param name="value">true : afficher / false : retirer</param>
    public void DisplayCancelHelpKey(bool value)
    {
        cancelHelpKey.SetActive(value);
    }


    /// <summary>
    /// Afficher ou non l'objet d'aide "Tile Movement"
    /// </summary>
    /// <param name="value">true : afficher / false : retirer</param>
    public void SetTileMovementObj(bool value)
    {
        tileMovementObj.SetActive(value);
    }

    /// <summary>
    /// Mets à jour l'affichage des vagues
    /// </summary>
    /// <param name="actualWave">vague actuelle</param>
    /// <param name="waveAmount">vague finale</param>
    public void UpdateWave(int actualWave, int waveAmount)
    {
        waveText.text = "Wave " + actualWave + '/' + waveAmount;
    }

    /// <summary>
    /// Affiche l'highlight du mode "Swap"
    /// </summary>
    public void HighlightSwap()
    {
        rotateHighlight.SetActive(false);
        swapHighlight.SetActive(true);
    }
    /// <summary>
    /// Affiche l'highlight du mode "Rotation"
    /// </summary>
    public void HighlightRotate()
    {
        rotateHighlight.SetActive(true);
        swapHighlight.SetActive(false);
    }
    /// <summary>
    /// Retire les highlights
    /// </summary>
    public void StopHighlight()
    {
        rotateHighlight.SetActive(false);
        swapHighlight.SetActive(false);
    }

}
