using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private int startingLife=20;
    private int life;

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

    public void DamageBase(int damage)
    {
        life -= damage;
    }

    public void DamageBase()
    {
        DamageBase(1);
    }

    public int GetLife()
    {
        return life;
    }
}
