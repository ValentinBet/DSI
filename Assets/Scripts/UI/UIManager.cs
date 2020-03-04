using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RectTransform allyHint;

    private bool isAllyHintFollowingMouse = false;

    private void Update()
    {
        if (isAllyHintFollowingMouse)
        {
            allyHint.transform.position = Input.mousePosition;
        }
    }

    public void SetAllyHintState(bool value)
    {
        allyHint.gameObject.SetActive(value);
        isAllyHintFollowingMouse = value;
    }
}
