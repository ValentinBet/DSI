using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> baseAllyList = new List<GameObject>();

    public List<GameObject> GetBaseAllyList()
    {
        List<GameObject> _baseAllies = new List<GameObject>();
        _baseAllies = baseAllyList;
        return _baseAllies;
    }

}
