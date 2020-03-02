using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    public Transform selector;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out hit))
        {
            if (hit.transform.gameObject.name =="obstacle")
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(0.726675f, 0, 1, 0.6705883f);
            }
            selector.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x)/4 *4+2, 0.5f, Mathf.RoundToInt(hit.point.z)/4 *4+2);
        }
    }
}
