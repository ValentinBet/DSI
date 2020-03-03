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

        if (Physics.Raycast(ray, out hit, 15000.0f, 9))
        {
            /*if (hit.transform.gameObject.name =="obstacle")
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(0.726675f, 0, 1, 0.6705883f);
            }*/
            selector.transform.position = hit.collider.transform.position + Vector3.up * 0.2f;
                //new Vector3(Mathf.RoundToInt(hit.point.x)/2 *2+1, 0.5f, Mathf.RoundToInt(hit.point.z)/2 *2+1);
        }
    }
}
