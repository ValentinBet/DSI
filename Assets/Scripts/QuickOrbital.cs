using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class QuickOrbital : MonoBehaviour
{
    private CinemachineOrbitalTransposer cot;

    private void Start()
    {
        cot = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            cot.m_XAxis.m_MaxSpeed = 300;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            cot.m_XAxis.m_MaxSpeed = 0;
        }
    }
}
