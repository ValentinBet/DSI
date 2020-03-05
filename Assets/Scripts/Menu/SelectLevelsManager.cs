using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelsManager : MonoBehaviour
{
    public void UnloadLevelsScene()
    {
        SceneManager.UnloadSceneAsync("SelectLevels");
    }
}
