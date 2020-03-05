using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersGenerator : MonoBehaviour
{
   [SerializeField] private List<GameObject> baseAllyList = new List<GameObject>();

    public List<GameObject> GetBaseAllyList()
    {
        List<GameObject> _baseAlly = new List<GameObject>();
        return _baseAlly = baseAllyList;
    }
}
