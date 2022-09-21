using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IncomeText : MonoBehaviour
{
    RectTransform rectTransform;
   [HideInInspector] public float y;
    public float changeY;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        y += changeY;
        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y+y, rectTransform.position.z);
        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 1 - y*10);

    }
}
