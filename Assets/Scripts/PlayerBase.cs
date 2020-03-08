using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{

    [SerializeField] private int startingLife=20;
    [SerializeField] private int life;

    private static PlayerBase _instance;
    public static PlayerBase Instance { get { return _instance; } }


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

        life = startingLife;
    }

    public void DamageBase(int damage = 1)
    {
        life -= damage;
        UIManager.Instance.BaseLifeUpdate(life);
    }

    public int GetLife()
    {
        return life;
    }
}
