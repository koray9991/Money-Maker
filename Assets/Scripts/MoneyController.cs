using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public GameObject moneyPrefab;
    [HideInInspector] public float randomMoneyTimer;
    [HideInInspector] public float generateMoneyTimer;
    //void Start()
    //{
    //    var newBuilding = Instantiate(moneyPrefab, transform.position, Quaternion.Euler(0, 0, 0));
    //    newBuilding.transform.parent = gameObject.transform;
    //}

   
    void Update()
    {
        if (transform.childCount == 0)
        {
            randomMoneyTimer = Random.Range(4f, 10f);
            if (randomMoneyTimer != 0)
            {
                generateMoneyTimer += Time.deltaTime;
                if (generateMoneyTimer >= randomMoneyTimer)
                {
                    var newBuilding = Instantiate(moneyPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
                    newBuilding.transform.parent = gameObject.transform;
                    randomMoneyTimer = 0;
                    generateMoneyTimer = 0;
                }
            }
        }
    }
}
