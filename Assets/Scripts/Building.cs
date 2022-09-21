using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Building : MonoBehaviour
{
   
    void Start()
    {
        transform.DOLocalMoveY(transform.parent.GetComponent<BuildingPlane>().buildingLevel*3.5f-3.5f, 1).SetEase(Ease.OutBounce);
    }

    
}
