using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField]
    private Transform camPivot;
    private CinemachineVirtualCamera thisCam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        thisCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ChangeCamPivot(Vector3 newPos)
    {
        camPivot.transform.position = newPos;
    }

    public void ChangeCamSize(float newSize)
    {
        thisCam.m_Lens.OrthographicSize = newSize;
    }
}
