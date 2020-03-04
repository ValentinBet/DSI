using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform camPivot;
    private CinemachineVirtualCamera thisCam;

    public static CameraManager Instance { get { return _instance; } }
    private static CameraManager _instance;

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
