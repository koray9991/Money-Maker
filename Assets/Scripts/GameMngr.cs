using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
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
    public GameObject buyButton;
    public Text buyButtonLevelText, buyButtonCostText;
    public bool buildingBought;
    public GameObject upgradeButton;
    public Text upgradeButtonLevelText, upgradeButtonCostText;
    [HideInInspector] public bool  upgraded;
    float timer;
    public int blueHomeCount, redHomeCount, greenHomeCount, yellowHomeCount, emptyHomeCount;
    [HideInInspector] public bool redCanEliminate,greenCanEliminate,yellowCanEliminate;
    [HideInInspector] public bool redDestroyed,greenDestroyed,yellowDestroyed;
    public GameObject redDestroyParticle,greenDestroyParticle,yellowDestroyParticle;
    public GameObject redCam, greenCam, yellowCam;
    public GameObject redPlayerEliminatedUI,greenPlayerEliminatedUI,yellowPlayerEliminatedUI;
     public GameObject winPanel;
    [HideInInspector] public bool winBool;
    public float bluePower, redPower, greenPower, yellowPover,totalPower;
    public Image bluePowerUI, redPowerUI, greenPowerUI, yellowPowerUI;
    public GameObject moneyParticle1, moneyParticle2;
    void Start()
    {
        //bankMoneyCount = 5000;
        //totalMoneyCount = 5000;
        Time.timeScale = 1;
        bankMoneyText.text =  bankMoneyCount + "$ ";
        totalMoneyText.text = totalMoneyCount.ToString();

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

        emptyHomeCount = 24;
        bluePower = 100;
        redPower = 100;
        greenPower = 100;
        yellowPover = 100;
        totalPower = 400;
    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(3f);
       // Time.timeScale = 0;
        winPanel.SetActive(true);
        moneyParticle1.SetActive(true);
        moneyParticle2.SetActive(true);

    }
    private void Update()
    {
        if(redDestroyed && greenDestroyed && yellowDestroyed && !winBool)
        {
            winBool = true;
            StartCoroutine(Win());
        }
        if(redPower==0 && greenPower==0 && yellowPover==0 && emptyHomeCount == 0)
        {
            winBool = true;
            StartCoroutine(Win());
        }
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            #region 
            timer = 0;
            emptyHomeCount = 0;
            bluePower = 100;
            redPower = 100;
            greenPower = 100;
            yellowPover = 100;
            blueHomeCount = 0;
            redHomeCount = 0;
            greenHomeCount = 0;
            yellowHomeCount = 0;
            #endregion
            GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("BuildingPlane");
            for (int i = 0; i < allBuildings.Length; i++)
            {
                if(allBuildings[i].GetComponent<BuildingPlane>().color == "")
                {
                    emptyHomeCount++;                
                }
                if (allBuildings[i].GetComponent<BuildingPlane>().color == "blue")
                {
                    blueHomeCount++;
                   
                    bluePower += allBuildings[i].GetComponent<BuildingPlane>().buildingLevel*100 +totalMoneyCount;


                }
                if (allBuildings[i].GetComponent<BuildingPlane>().color == "red")
                {
                    redHomeCount++;
                    if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerRed");
                        redPower += allBuildings[i].GetComponent<BuildingPlane>().buildingLevel * 100 + player.GetComponent<Ai>().totalMoneyCount;

                    }
                   

                    
                }
                if (allBuildings[i].GetComponent<BuildingPlane>().color == "green")
                {
                    greenHomeCount++;
                    if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerGreen");
                        greenPower += allBuildings[i].GetComponent<BuildingPlane>().buildingLevel * 100 + player.GetComponent<Ai>().totalMoneyCount;

                    }
                  
                }
                if (allBuildings[i].GetComponent<BuildingPlane>().color == "yellow")
                {
                    yellowHomeCount++;
                    if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerYellow");
                        yellowPover += allBuildings[i].GetComponent<BuildingPlane>().buildingLevel * 100 + player.GetComponent<Ai>().totalMoneyCount;

                    }
                }

                if (GameObject.FindGameObjectWithTag("PlayerRed") == null)
                {
                    redPower = 0;
                    redDestroyed = true;
                }
                if (GameObject.FindGameObjectWithTag("PlayerGreen") == null)
                {
                    greenPower = 0;
                    greenDestroyed = true;
                }
                if (GameObject.FindGameObjectWithTag("PlayerYellow") == null)
                {
                    yellowPover = 0;
                    yellowDestroyed = true;
                }
                if (blueHomeCount == 0)
                {
                    bluePower = 100+ totalMoneyCount;
                }
                if (redHomeCount == 0)
                {
                    if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerRed");
                        redPower = 100+player.GetComponent<Ai>().totalMoneyCount;

                    }
                }
                if (greenHomeCount == 0)
                {
                    if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerGreen");
                        greenPower = 100 + player.GetComponent<Ai>().totalMoneyCount;

                    }
                }
                if (yellowHomeCount == 0)
                {
                    if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerYellow");
                        yellowPover= 100 + player.GetComponent<Ai>().totalMoneyCount;

                    }
                }
                totalPower = bluePower + yellowPover + greenPower + redPower;
                bluePowerUI.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
                bluePowerUI.GetComponent<RectTransform>().anchorMax = new Vector2(bluePower / totalPower, 0.5f);

                redPowerUI.GetComponent<RectTransform>().anchorMin = new Vector2(bluePower / totalPower, 0.5f);
                redPowerUI.GetComponent<RectTransform>().anchorMax = new Vector2(bluePower / totalPower + redPower / totalPower, 0.5f);

                greenPowerUI.GetComponent<RectTransform>().anchorMin = new Vector2(bluePower / totalPower + redPower / totalPower, 0.5f);
                greenPowerUI.GetComponent<RectTransform>().anchorMax = new Vector2(bluePower / totalPower + redPower / totalPower + greenPower / totalPower, 0.5f);

                yellowPowerUI.GetComponent<RectTransform>().anchorMin = new Vector2(bluePower / totalPower + redPower / totalPower + greenPower / totalPower, 0.5f);
                yellowPowerUI.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);

            }
            if (redHomeCount > 0)
            {
                redCanEliminate = true;
            }
            if (redCanEliminate && redHomeCount==0)
            {
                var player = GameObject.FindGameObjectWithTag("PlayerRed");
                Destroy(player,1);
                StartCoroutine(redDestroy());
                redCam.GetComponent<CinemachineVirtualCamera>().Priority = 20;
                StartCoroutine(CamDefault());
                redCanEliminate = false;
                player.GetComponent<Ai>().bankMoneyText.enabled = false;
                redDestroyed = true;
            }
            if (greenHomeCount > 0)
            {
                greenCanEliminate = true;
            }
            if (greenCanEliminate && greenHomeCount == 0)
            {
                var player = GameObject.FindGameObjectWithTag("PlayerGreen");
                Destroy(player, 1);
                StartCoroutine(greenDestroy());
                greenCam.GetComponent<CinemachineVirtualCamera>().Priority = 20;
                StartCoroutine(CamDefault());
                greenCanEliminate = false;
                player.GetComponent<Ai>().bankMoneyText.enabled = false;
                greenDestroyed = true;
            }
            if (yellowHomeCount > 0)
            {
                yellowCanEliminate = true;
            }
            if (yellowCanEliminate && yellowHomeCount == 0)
            {
                var player = GameObject.FindGameObjectWithTag("PlayerYellow");
                Destroy(player, 1);
                StartCoroutine(yellowDestroy());
                yellowCam.GetComponent<CinemachineVirtualCamera>().Priority = 20;
                StartCoroutine(CamDefault());
                yellowCanEliminate = false;
                yellowDestroyed = true;
                player.GetComponent<Ai>().bankMoneyText.enabled = false;
            }
        }
    }
    IEnumerator redDestroy()
    {
        yield return new WaitForSeconds(0.95f);
       var particle= Instantiate(redDestroyParticle, GameObject.FindGameObjectWithTag("PlayerRed").transform.position , Quaternion.identity);
        redPlayerEliminatedUI.SetActive(true);
        redCam.GetComponent<CinemachineVirtualCamera>().Follow = particle.transform;
        redCam.GetComponent<CinemachineVirtualCamera>().LookAt = particle.transform;
    }
    IEnumerator greenDestroy()
    {
        yield return new WaitForSeconds(0.95f);
        var particle = Instantiate(greenDestroyParticle, GameObject.FindGameObjectWithTag("PlayerGreen").transform.position , Quaternion.identity);
        greenPlayerEliminatedUI.SetActive(true);
        greenCam.GetComponent<CinemachineVirtualCamera>().Follow = particle.transform;
        greenCam.GetComponent<CinemachineVirtualCamera>().LookAt = particle.transform;
    }
    IEnumerator yellowDestroy()
    {
        yield return new WaitForSeconds(0.95f);
        var particle = Instantiate(yellowDestroyParticle, GameObject.FindGameObjectWithTag("PlayerYellow").transform.position , Quaternion.identity);
        yellowPlayerEliminatedUI.SetActive(true);
        yellowCam.GetComponent<CinemachineVirtualCamera>().Follow = particle.transform;
        yellowCam.GetComponent<CinemachineVirtualCamera>().LookAt = particle.transform;
    }
    IEnumerator CamDefault()
    {
        
        yield return new WaitForSeconds(2);
        redCam.GetComponent<CinemachineVirtualCamera>().Priority = 2;
        greenCam.GetComponent<CinemachineVirtualCamera>().Priority = 2;
        yellowCam.GetComponent<CinemachineVirtualCamera>().Priority = 2;
        redPlayerEliminatedUI.SetActive(false);
        greenPlayerEliminatedUI.SetActive(false);
        yellowPlayerEliminatedUI.SetActive(false);
    }
    public void Buttons(int buttonNo)
    {
        if (buttonNo == 1)
        {
            if (bankMoneyCount >= speedUpgradeCost)
            {

                bankMoneyCount -= speedUpgradeCost;
                totalMoneyCount -= speedUpgradeCost;
                totalMoneyText.text = totalMoneyCount.ToString();
                bankMoneyText.text = bankMoneyCount + "$ ";
                for (int i = 0; i < speedUpgradeCost; i++)
                {
                    me.bankStackMode -= 1;
                    if (me.bankStackMode == -1)
                    {
                        me.bankStackMode = 8;
                    }
                    me.bankStack[me.bankStackMode].transform.localScale += new Vector3(0, 0, -me.moneyScaleChange);
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                }
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
                totalMoneyText.text = totalMoneyCount.ToString();
                bankMoneyText.text = bankMoneyCount + "$ ";
                for (int i = 0; i < stackUpgradeCost; i++)
                {
                    me.bankStackMode -= 1;
                    if (me.bankStackMode == -1)
                    {
                        me.bankStackMode = 8;
                    }
                    me.bankStack[me.bankStackMode].transform.localScale += new Vector3(0, 0, -me.moneyScaleChange);
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                }
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
                totalMoneyText.text = totalMoneyCount.ToString();
                bankMoneyText.text = bankMoneyCount + "$ ";
                for (int i = 0; i < incomeUpgradeCost; i++)
                {
                    me.bankStackMode -= 1;
                    if (me.bankStackMode == -1)
                    {
                        me.bankStackMode = 8;
                    }
                    me.bankStack[me.bankStackMode].transform.localScale += new Vector3(0, 0, -me.moneyScaleChange);
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                }
                incomeLevel += 1;
                incomeUpgradeCost += incomeUpgradeCost * 0.75f;
                if (incomeUpgradeCost % 1 != 0)
                {
                    var i = incomeUpgradeCost % 1;
                    incomeUpgradeCost -= i;
                }
                incomeUpgradeCostText.text = incomeUpgradeCost + " $";
                incomeLevelText.text = "LEVEL " + incomeLevel;
                //using in BuildingPlane.

               
            }


        }
        if (buttonNo == 4)
        {
            if (bankMoneyCount >= me.buyCost)
            {
                bankMoneyCount -= me.buyCost;
                totalMoneyCount -= me.buyCost;
                totalMoneyText.text = totalMoneyCount.ToString();
                bankMoneyText.text = bankMoneyCount + "$ ";
                buildingBought = true;

                for (int i = 0; i < me.buyCost; i++)
                {
                   me.bankStackMode -= 1;
                    if (me.bankStackMode == -1)
                    {
                        me.bankStackMode = 8;
                    }
                    me.bankStack[me.bankStackMode].transform.localScale += new Vector3(0, 0, -me.moneyScaleChange);
                   me. bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, me.bankStack[me.bankStackMode].transform.localScale.z / 300));
                    me.bankStack[me.bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1,me. bankStack[me.bankStackMode].transform.localScale.z / 300));
                }

               

            }
        }
        if (buttonNo == 5)
        {
            if (bankMoneyCount >= 0)
            {
                upgraded = true;
            }
        }
        if(buttonNo == 6)
        {
            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

   
}
