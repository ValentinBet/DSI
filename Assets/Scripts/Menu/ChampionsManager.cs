using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChampionsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> TabElement = new List<GameObject>();
    [SerializeField] private Color activatedTabElementColor;
    [SerializeField] private Color desactivatedTabElementColor;

    private void ResetTabElementColor()
    {
        foreach (GameObject TbElmt in TabElement)
        {
            TbElmt.GetComponent<Image>().color = desactivatedTabElementColor;
        }
    }

    public void ActivateTabElement(int value)
    {
        ResetTabElementColor();
        TabElement[value].GetComponent<Image>().color = activatedTabElementColor;

        UpdateTab(value);
    }

    private void UpdateTab(int value)
    {

    }

    public void UnloadChampionsScene()
    {
        SceneManager.UnloadSceneAsync("Champions");
    }


}
