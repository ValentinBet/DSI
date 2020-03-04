using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{

    public float maxScreenShakeIntensity = 10;

    [SerializeField]
    private Transform camPivot;
    private CinemachineVirtualCamera thisCam;
    private bool isScreenShaking = false;

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

    public void InitShakeScreen(float intensity = 2, float duration = 0.1f)
    {
        intensity = Mathf.Clamp(intensity, 0, maxScreenShakeIntensity);

        if (!isScreenShaking)
        {
            StartCoroutine(ProcessShake(intensity, duration));
        }
    }

    private IEnumerator ProcessShake(float intensity, float duration)
    {
        isScreenShaking = true;
        CamerasShake(intensity);
        yield return new WaitForSeconds(duration);
        CamerasShake(0);
        isScreenShaking = false;
    }

    private void CamerasShake(float intensity)
    {
        thisCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        thisCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = intensity;        
    }
}
