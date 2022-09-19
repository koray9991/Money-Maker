using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI levelText;
    public GameObject Hero;
    public GameObject StartButton;
    public GameObject ShopPanel;
    public GameObject FinishPanel;
    public GameObject levelTutorial;
    public List<GameObject> levelList;


    public Color skycolor = new Color(1, 1, 1);
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timerText;
    public float MoneyAmount;
    public float SafeMoney;
    public float BodyMoney;
    public GameObject moneyIcon;
    private bool isMoneyAnimate = true;
    public bool isStart = false;
    public bool isFinish = false;
    public bool isTutorial = false;
    public float customerFeedback = 0f;
  
    public GameObject tutorialPanel;
    public int levelNumber = 0;
    public int cNum = 0;
    public GameObject panel;






    public TextMeshProUGUI safeMoneyText;

    void Awake()
    {
        //        GameAnalytics.Initialize();
    }
    void Start()
    {
        PlayerPrefs.SetFloat("TotalMoney", 0);
     

        levelNumber = PlayerPrefs.GetInt("LevelNumber");
   
        MoneyAmount = PlayerPrefs.GetFloat("TotalMoney");

        MoneyUpdate(0);
   

    }



   /* public void StartLevel()
    {
        Hero.GetComponent<Hero>().LevelStart();
        levelTutorial.SetActive(false);

        levelNumber = PlayerPrefs.GetInt("LevelNumber");
        levelList[levelNumber % 5].SetActive(true);
        //GameAnalytics.NewDesignEvent("levelnumber", (float)levelNumber);
        isTutorial = false;
        //GameObject.Find("Gate").GetComponent<Gate>().OpenGate();
        LeanTween.moveY(ShopPanel.GetComponent<RectTransform>(), -1000, 0.5f);

        isStart = true;
        Hero.GetComponent<Hero>().isStart = true;
        StartButton.SetActive(false);
        //CreateCustomer();
    }

    public void StartTutorial()
    {
        levelText.text = "How To Play";
        for (var i = 0; i < levelList.Count; i++)
        {
            levelList[i].SetActive(false);
        }

        levelTutorial.SetActive(true);
        isTutorial = true;
        LeanTween.moveY(ShopPanel.GetComponent<RectTransform>(), -1000, 0.5f);
        isStart = true;
        Hero.GetComponent<Hero>().isStart = true;
        StartButton.SetActive(false);
        tutorialPanel.SetActive(true);

    }*/

    /*
    public GameObject CreateCustomer(GameObject wayPoint)
    {

        customerList.Add(null);
        customerList[cNum] = Instantiate(customerPrefab, new Vector3(10, 0, -22), Quaternion.Euler(0, -150, 0));
        int gender = 2;// Random.Range(1, 3);
        customerList[cNum].GetComponent<Npc>().gender = gender;
        int renk1 = Random.Range(0, 17);
        int hairRenk = Random.Range(0, 5);
        int bodyRenk = Random.Range(0, bodyPalet.Length);
        customerList[cNum].GetComponent<Npc>().body.GetComponent<Renderer>().material.color = bodyPalet[bodyRenk];
        customerList[cNum].GetComponent<Npc>().Target(wayPoint);

        if (gender == 1)
        {
            int mayotip = Random.Range(1, 4);
            int hairtip = Random.Range(0, 4);
            int moustachetip = Random.Range(1, 6);
            customerList[cNum].GetComponent<Npc>().short1.SetActive(true);
            customerList[cNum].GetComponent<Npc>().short1.GetComponent<Renderer>().material.color = colorPalet[renk1 % 15];
            customerList[cNum].GetComponent<Npc>().hairList[hairtip].SetActive(true);
            customerList[cNum].GetComponent<Npc>().hairList[hairtip].GetComponent<Renderer>().material.color = hairPalet[hairRenk];
            if (moustachetip == 3)
            {
                customerList[cNum].GetComponent<Npc>().moustache1.SetActive(true);
                customerList[cNum].GetComponent<Npc>().moustache1.GetComponent<Renderer>().material.color = hairPalet[hairRenk];
            }
            if (mayotip == 1)
            {
                customerList[cNum].GetComponent<Npc>().tshirt1.SetActive(false);
                customerList[cNum].GetComponent<Npc>().tshirt1.GetComponent<Renderer>().material.color = colorPalet[(renk1 + mayotip + 1) % 15];
            }
            else
            {
                customerList[cNum].GetComponent<Npc>().tshirt1.SetActive(false);

            }
        }
        else
        {
            int mayotip = Random.Range(1, 3);
            int hairtip = Random.Range(4, 7);
            customerList[cNum].GetComponent<Npc>().hairList[hairtip].SetActive(true);
            customerList[cNum].GetComponent<Npc>().hairList[hairtip].GetComponent<Renderer>().material.color = hairPalet[hairRenk];
            if (mayotip == 1)
            {
                customerList[cNum].GetComponent<Npc>().mayo1.SetActive(true);
                customerList[cNum].GetComponent<Npc>().mayo1.GetComponent<Renderer>().material.color = colorPalet[renk1 % 15];
            }
            else
            {
                customerList[cNum].GetComponent<Npc>().mayo2.SetActive(true);
                customerList[cNum].GetComponent<Npc>().mayo2.GetComponent<Renderer>().material.color = colorPalet[renk1 % 15];
            }
        }

        GameObject customer = customerList[cNum].gameObject;

        cNum++;
        return customer;
    }*/

    public void RestartLevel()
    {
        SceneManager.LoadScene("Scene2");
    }

    public void SuccessLevel()
    {
        RestartLevel();
    }
    public void FailLevel()
    {
        RestartLevel();
    }

    public void MoneyUpdate(float miktar)
    {
        MoneyAmount = MoneyAmount + miktar;
        

        if (miktar < 0)
        {
            if (BodyMoney > miktar * -1)
            {
                BodyMoney = BodyMoney + miktar;
            }
            else
            {
                SafeMoney = SafeMoney + (BodyMoney + miktar);
                BodyMoney = 0;
            }

    
            Hero.GetComponent<Hero>().MoneyDecrease(BodyMoney);
         
        }
        else
        {
            BodyMoney = BodyMoney + miktar;
        }
        
        if (MoneyAmount < 0) MoneyAmount = 0f;
        //moneyText.text = "$" + MoneyAmount.ToString();
        safeMoneyText.text = "$ " + ((int)SafeMoney).ToString();
        // LeanTween.cancel(moneyIcon);
        // LeanTween.scale(moneyIcon.GetComponent<RectTransform>(), new Vector3(2f, 2f, 2f), 0.1f).setLoopPingPong(1);
        //  LeanTween.delayedCall(gameObject, 0.11f, () => { LeanTween.cancel(moneyIcon); moneyIcon.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); });
        PlayerPrefs.SetFloat("TotalMoney", MoneyAmount);

    }

    //public void ShopMoneyUpdate(float miktar)
    //{
    //    MoneyAmount = MoneyAmount + miktar;
    //    if (MoneyAmount < 0) MoneyAmount = 0f;
    //    PlayerPrefs.SetFloat("TotalMoney", MoneyAmount);
    //    moneyText.text = "$" + MoneyAmount.ToString();
    //    LeanTween.cancel(moneyIcon);
    //    LeanTween.scale(moneyIcon.GetComponent<RectTransform>(), new Vector3(2f, 2f, 2f), 0.1f).setLoopPingPong(1);
    //    LeanTween.delayedCall(gameObject, 0.11f, () => { LeanTween.cancel(moneyIcon); moneyIcon.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); });
    //}





    public string ExchangeMoney(float amount1)
    {
        string textAmount = "";
        if (amount1 >= 1000)
        {
            if (Mathf.Floor(amount1 / 1000) < (amount1 / 1000))
            {
                textAmount = "$" + (amount1 / 1000).ToString("N1") + "K";
            }
            else
            {
                textAmount = "$" + (amount1 / 1000).ToString("N0") + "K";
            }
        }
        else
        {
            textAmount = "$" + amount1.ToString("N0");
        }
        return textAmount;
    }



    public void PanelOpen()
    {
        LeanTween.moveY(panel.GetComponent<RectTransform>(), -150, 0.15f);

    }
    public void PanelClose()
    {
        LeanTween.moveY(panel.GetComponent<RectTransform>(), -3000, 0.15f);
    }
}