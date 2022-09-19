using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoneySpawn : MonoBehaviour
{
    public GameObject moneyPrefab;
    public int moneyNum;
    public List<GameObject> moneyList;
   
    public float speed= 4f;
    public int maxLimit = 12;
    
    public bool isWork = false;
    void Start()
    {      
        moneyNum = 0;
    }


    void Update()
    {
        if (moneyNum < maxLimit && isWork == false)
        {
            isWork = true;
            LeanTween.delayedCall(gameObject, speed, () =>
            {

                GameObject money1 = Instantiate(moneyPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);

                
                    float moneyRow = moneyNum / 2;
                    money1.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    if (moneyNum % 2 == 0) { money1.transform.localPosition = new Vector3(0, (moneyRow / 6) + 0.2f, 0.25f); }
                    else { money1.transform.localPosition = new Vector3(0, (moneyRow / 6) + 0.2f, -0.25f); }
                

                moneyList.Add(money1);
                moneyNum++;
                isWork = false;

            });
        }
    }



}
