using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCharacter : Character
{
    public AllyCharacterData data;

    private void Start()
    {
        gameObject.tag = "AllyCharacter";
    }
}
