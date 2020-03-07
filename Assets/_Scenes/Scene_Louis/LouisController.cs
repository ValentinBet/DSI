using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LouisController : MonoBehaviour
{
    public Character[] characters;
    private int currentIndex;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (characters[0] != null)
            {

                PatternReader.instance.PatternExecuter.ExecutePattern(characters[0].mouvementPattern, characters[0]);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PatternReader.instance.PatternExecuter.ExecutePattern(characters[currentIndex].mouvementPattern, characters[currentIndex]);
            currentIndex++;
            if (currentIndex >= characters.Length)
            {
                currentIndex = 0;
            }
        }
    }

}
