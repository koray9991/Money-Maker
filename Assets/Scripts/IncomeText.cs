using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IncomeText : MonoBehaviour
{
    Transform tr;
   // RectTransform rectTransform;
   [HideInInspector] public float y;
    public float changeY;
    public float rotate;
    public float destroyTimer;
    void Start()
    {
        //rectTransform = GetComponent<RectTransform>();
        tr = GetComponent<Transform>();
        tr.localRotation = Quaternion.Euler(90, 0, 0);
        Destroy(gameObject, destroyTimer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        y += changeY;
        tr.position = new Vector3(tr.position.x, tr.position.y+y, tr.position.z);
       // GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 1 - y*5);
        transform.Rotate(0, 0, -rotate);
    }
}
