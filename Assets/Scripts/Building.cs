using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Building : MonoBehaviour
{
    
    void Start()
    {
        transform.DOLocalMoveY(transform.parent.GetComponent<BuildingPlane>().buildingLevel*4-4, 1).SetEase(Ease.OutBounce);
    }

    
}
