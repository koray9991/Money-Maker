using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Table : MonoBehaviour
{
    public GameObject table;
    public GameObject buyArea;
    public GameObject moneyPrefab;
    public bool isMoney = false;
    public bool isDrink = false;
    public bool isFastFood = false;
    public bool isBuy = false;    
    public float speed = 4f;
    public bool isWork = false;
    public GameObject nova;
    public float buyAmount = 50;
    public float buySpeed = 3f;
    public float buyTime = 0;
    public float amount = 0;
    public TextMeshProUGUI amountLabel;
    private GameManager _gm;
    public Image buySlider;
    public GameObject wayPoint;
    public int oldDiv1 = 0;
    
    public GameObject moneySpawnParent;
    public GameObject drink;
    public float drinkSpeed =5f;
    public GameObject fastfood;
    public float fastFoodSpeed = 5f;
    public GameObject customer;
    public bool isCustomer = false;
    AudioSource m_AudioSource;









    public List<GameObject> buildingList;
    public float buildingLevel;

    void Start()
    {
        //m_AudioSource = GetComponent<AudioSource>();
        _gm = FindObjectOfType<GameManager>();
        amountLabel.text = _gm.ExchangeMoney(buyAmount);
      
    }



   /* public void SpawnDrinkMoney()
    {

        for (int t = 0; t < 2; t++)
        {
            int moneyNum = moneySpawnParent.GetComponent<MoneySpawn>().moneyNum;
            int maxLimit = moneySpawnParent.GetComponent<MoneySpawn>().maxLimit;

            if (moneyNum < maxLimit)
            {
                GameObject money1 = Instantiate(moneyPrefab, new Vector3(0, 0, 0), Quaternion.identity, moneySpawnParent.transform);
                float moneyRow = moneyNum / 2;
                money1.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                if (moneyNum % 2 == 0) { money1.transform.localPosition = new Vector3(0.5f, (moneyRow / 6) + 0.2f, 0.5f); }
                else { money1.transform.localPosition = new Vector3(0.5f, (moneyRow / 6) + 0.2f, -0.5f); }
                moneySpawnParent.GetComponent<MoneySpawn>().moneyList.Add(money1);
                moneySpawnParent.GetComponent<MoneySpawn>().moneyNum++;
            }
        }

    }*/

    public void LevelUp()
    {
        buildingLevel++;
        isBuy = true;
        nova.SetActive(true);
       
        var newBuilding = Instantiate(buildingList[0], transform.position + new Vector3(0,  buildingLevel/2, 0f), Quaternion.identity);
        newBuilding.transform.parent = gameObject.transform;
        newBuilding.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(newBuilding, new Vector3(1, 1, 1), 0.2f).setEaseInBounce();
        amount = 0;
        buyAmount = buildingLevel*100 + buyAmount;
        buySlider.fillAmount = amount / buyAmount;
        amountLabel.text = "$ " +buyAmount.ToString();
        buyTime = 0;
    }



  /*  public void CustomerStart() {
        moneySpawnParent.SetActive(true);
        isCustomer = true;
    }*/

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" )
        {      
            if (_gm.MoneyAmount > 0  && other.GetComponent<Hero>().pressed == false)
            {
                if (buyTime >= buySpeed)
                {
                   
                    LevelUp();
                } else {
                    if (_gm.MoneyAmount > (Time.deltaTime / buySpeed) * buyAmount)
                    {
                        buyTime = buyTime + Time.deltaTime;
                        amount = amount + ((Time.deltaTime / buySpeed) * buyAmount);
                        amountLabel.text = _gm.ExchangeMoney(buyAmount - amount);
                        
                        _gm.MoneyUpdate((Time.deltaTime /buySpeed) * -buyAmount);
                    }
                    if (_gm.MoneyAmount > 0 && _gm.MoneyAmount < (Time.deltaTime/ buySpeed) * buyAmount)
                    {
                        buyTime = buyTime + Time.deltaTime;
                      
                           amount = amount + (_gm.MoneyAmount);
                        amountLabel.text = _gm.ExchangeMoney(buyAmount - amount);
                        _gm.MoneyUpdate(_gm.MoneyAmount * -1);
                        
                    }
                }
                buySlider.fillAmount = amount / buyAmount;
            }
            if (buyAmount - amount <= 0) {
                isBuy = true;
                LevelUp();
            }


        }
    }



}

