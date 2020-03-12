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
        "Kaaris"
    };

    public List<GameObject> GetBaseAllyList()
    {
        List<GameObject> _baseAllies = new List<GameObject>();

        _baseAllies = baseAllyList;
        return _baseAllies;
    }

    public AllyCharacterSave GetNewCharacterSave(CharacterType allyType = CharacterType.Warrior)
    {
        AllyCharacterSave newCharacter = new AllyCharacterSave();

        return SetBasicStats(newCharacter, allyType);
    }


    private GameObject GetBaseAllyByType(CharacterType allyType = CharacterType.Warrior)
    {
        int index = 0;

        switch (allyType)
        {
            case CharacterType.Warrior:
                index = 0;
                break;
            case CharacterType.Archer:
                index = 1;
                break;
            case CharacterType.Mage:
                index = 2;
                break;
            default:
                index = 0;
                break;
        }

        return baseAllyList[index];
    }

    public AllyCharacterSave SetBasicStats(AllyCharacterSave ANewGameAlly, CharacterType allyType = CharacterType.Warrior)
    {
        AllyCharacter baseCharacterStats = GetBaseAllyByType(allyType).GetComponent<AllyCharacter>();

        ANewGameAlly.name = GetRandomName();
        ANewGameAlly.damage = baseCharacterStats.damage;
        ANewGameAlly.life = baseCharacterStats.life;
        ANewGameAlly.AttackRange = baseCharacterStats.AttackRange;
        ANewGameAlly.movementRange = baseCharacterStats.movementRange;
        ANewGameAlly.type = allyType;

        return ANewGameAlly;
    }

    public static string GetRandomName()
    {
        return baseAllyName[Random.Range(0, baseAllyName.Count)];
    }

}
