using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMngr : MonoBehaviour
{
    public Character me;
    public  float stackedMoneyCount;
    public float totalMoneyCount, bankMoneyCount;
    public float maxStackedCount;
    public Text bankMoneyText, stackedMoneyText, totalMoneyText;
    public GameObject upgradesPanel;
    public float speedUpgradeCost, stackUpgradeCost, incomeUpgradeCost;
    public float speedLevel, stackLevel, incomeLevel;
    public Text speedLevelText, stackLevelText, incomeLevelText;
    public Text speedUpgradeCostText, stackUpgradeCostText, incomeUpgradeCostText;
    void Start()
    {
        bankMoneyText.text =  bankMoneyCount + "$ ";
        

        speedUpgradeCostText.text = speedUpgradeCost + " $";
        speedLevel = 1;
        speedLevelText.text = "LEVEL " + speedLevel;

        stackUpgradeCostText.text = stackUpgradeCost + " $";
        stackLevel = 1;
        stackLevelText.text = "LEVEL " + stackLevel;

        incomeUpgradeCostText.text = incomeUpgradeCost + " $";
        incomeLevel = 1;
        incomeLevelText.text = "LEVEL " + incomeLevel;

        upgradesPanel.SetActive(false);


    }
   public void Buttons(int buttonNo)
    {
        if (buttonNo == 1)
        {
            if (bankMoneyCount >= speedUpgradeCost)
            {
                bankMoneyCount -= speedUpgradeCost;
                totalMoneyCount -= speedUpgradeCost;
                bankMoneyText.text = bankMoneyCount + "$ ";

                speedLevel += 1;
                speedUpgradeCost +=((int) speedUpgradeCost * 0.75f);
                if (speedUpgradeCost % 1 != 0)
                {
                    var i = speedUpgradeCost % 1;
                    speedUpgradeCost -= i;
                }
                speedUpgradeCostText.text = speedUpgradeCost + " $";
                speedLevelText.text = "LEVEL " + speedLevel;
                me.GetComponent<Character>().moveSpeed += 0.5f;
            }
           

        }
       
        if (buttonNo == 2)
        {
            if (bankMoneyCount >= stackUpgradeCost)
            {
                bankMoneyCount -= stackUpgradeCost;
                totalMoneyCount -= stackUpgradeCost;
                bankMoneyText.text = bankMoneyCount + "$ ";

                stackLevel += 1;
                stackUpgradeCost += stackUpgradeCost * 0.75f;
                if (stackUpgradeCost % 1 != 0)
                {
                    var i = stackUpgradeCost % 1;
                    stackUpgradeCost -= i;
                }
                stackUpgradeCostText.text = stackUpgradeCost + " $";
                stackLevelText.text = "LEVEL " + stackLevel;
                maxStackedCount += 2;
            }


        }
        if (buttonNo == 3)
        {
            if (bankMoneyCount >= incomeUpgradeCost)
            {
                bankMoneyCount -= incomeUpgradeCost;
                totalMoneyCount -= incomeUpgradeCost;
                bankMoneyText.text = bankMoneyCount + "$ ";

                incomeLevel += 1;
                incomeUpgradeCost += incomeUpgradeCost * 0.75f;
                if (incomeUpgradeCost % 1 != 0)
                {
                    var i = incomeUpgradeCost % 1;
                    incomeUpgradeCost -= i;
                }
                incomeUpgradeCostText.text = incomeUpgradeCost + " $";
                incomeLevelText.text = "LEVEL " + incomeLevel;
                //using in BuildingPlane.cs
            }


        }
    }

   
}
