using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public RectTransform allyHint;
    public Image allyHintImg;

    private bool isAllyHintFollowingMouse = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if (isAllyHintFollowingMouse)
        {
            allyHint.transform.position = Input.mousePosition;
        }
    }

    public void SetAllyHintState(bool value, Sprite CharacterSprite = null)
    {
        allyHint.gameObject.SetActive(value);
        isAllyHintFollowingMouse = value;

        if (CharacterSprite != null)
        {
            SetAllyHintImg(CharacterSprite);
        }
    }

    public void SetAllyHintImg(Sprite CharacterSprite)
    {
        allyHintImg.sprite = CharacterSprite;
    }

}
