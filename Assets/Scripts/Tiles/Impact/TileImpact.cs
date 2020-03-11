using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileImpact : MonoBehaviour
{
    public AnimationCurve multiplier = AnimationCurve.Linear(0, 0, 1, 1);
    [Range(0.1f, 3f)]
    public float totalAnimationTime = 0.2f;

    private bool impactEnable;
    private Transform _transform;
    private float timer;
    private float _impactValue;
    private float initialPosY;

    private void Awake()
    {
        if (_transform == null)
        {
            _transform = transform;
        }
    }


    public void ActivateImpact(float impactValue)
    {
        impactEnable = true;
        timer = 0f;
        _impactValue = impactValue;
        initialPosY = _transform.position.y;

        StartCoroutine(ContinueUpdate());
    }

    private void UpdatePos()
    {
        float yDisplacment = initialPosY + (multiplier.Evaluate(timer / totalAnimationTime)) * _impactValue;
        _transform.position = new Vector3(_transform.position.x, yDisplacment, _transform.position.z);
    }

    private IEnumerator ContinueUpdate()
    {
        while (impactEnable)
        {
            timer += Time.deltaTime;
            if (timer / totalAnimationTime > 1)
            {
                DesactiveImpact();
            }
            UpdatePos();
            yield return new WaitForFixedUpdate();
            //
        }
    }

    public void DesactiveImpact()
    {
        impactEnable = false;
        StopAllCoroutines();
    }



}

