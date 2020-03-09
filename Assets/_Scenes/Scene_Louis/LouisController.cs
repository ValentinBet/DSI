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
                characters[0].myState = CharacterState.Standby;
                PatternReader.instance.PatternExecuter.ReadPattern(characters[0]);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            characters[currentIndex].myState = CharacterState.Standby;
            PatternReader.instance.PatternExecuter.ReadPattern(characters[currentIndex]);
            currentIndex++;
            if (currentIndex >= characters.Length)
            {
                currentIndex = 0;
            }
        }
    }

}
