using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> baseAllyList = new List<GameObject>();

    [SerializeField]
    private static List<string> baseAllyName = new List<string>()
    {
        "Anibal",
        "Adon",
        "Arvad",
        "Astartus",
        "Aqhat",
        "Philosir",
        "Amilcare",
        "Baal",
        "Zimrida",
        "Abdosir",
        "Gebal",
        "Baltazar",
        "Adad",
        "Eshmun",
        "Awil",
        "Shamash",
        "Ur",
        "Amnanu",
        "Nangish",
        "Mesh-He",
        "Mil-La-El",
        "Nanum",
        "Shalim",
        "Hutt",
        "Rihat",
        "Kinaa",
        "Arish",
        "Timgir",
        "Deemeth",
        "Mannui",
        "Shamash-Nasir",
        "Rimush",
        "Sharri",
        "Saad Pursnani",
        "Dakhil",
        "Basim",
        "Wakalat",
        "Nafis",
        "Bakkar",
        "Damurah",
        "Fakih",
        "Mamenos",
        "Mamenosh"
    };

    public List<GameObject> GetBaseAllyList()
    {
        List<GameObject> _baseAllies = new List<GameObject>();

        _baseAllies = baseAllyList;
        return _baseAllies;
    }

    public AllyCharacterSave SetBasicStats(AllyCharacterSave ANewGameAlly, AllyType allyType = AllyType.Warrior)
    {
        int index = 0;

        switch (allyType)
        {
            case AllyType.Warrior:
                index = 0;
                break;
            case AllyType.Archer:
                index = 1;
                break;
            case AllyType.Mage:
                index = 2;
                break;
            default:
                index = 0;
                break;
        }
        AllyCharacter baseCharacterStats = baseAllyList[index].GetComponent<AllyCharacter>();
        ANewGameAlly.name = GetRandomName();
        ANewGameAlly.damage = baseCharacterStats.damage;
        ANewGameAlly.life = baseCharacterStats.damage;
        ANewGameAlly.AttackRange = baseCharacterStats.damage;
        ANewGameAlly.movementRange = baseCharacterStats.damage;
        ANewGameAlly.type = allyType;

        return ANewGameAlly;
    }

    public static string GetRandomName()
    {
        return baseAllyName[Random.Range(0, baseAllyName.Count)];
    }

}
