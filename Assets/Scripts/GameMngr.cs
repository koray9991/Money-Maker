using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMngr : MonoBehaviour
{
   
    public  float stackedMoneyCount;
    public float totalMoneyCount, bankMoneyCount;
    public float maxStackedCount;
    public Text bankMoneyText, stackedMoneyText, totalMoneyText;
    void Start()
    {
        stackedMoneyText.text = "Stacked : " + stackedMoneyCount;
        totalMoneyText.text = "Total : " + totalMoneyCount;
        bankMoneyText.text = "Bank " + bankMoneyCount;
    }

   
}
