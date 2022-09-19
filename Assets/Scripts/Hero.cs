using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Hero : MonoBehaviour
{
    public Camera OrtoCam;
    public GameManager _gm; 
    public CinemachineVirtualCamera Vcam1;
    public CinemachineVirtualCamera Vcam2;
    public CinemachineVirtualCamera Vcam3;
    public CinemachineVirtualCamera Vcam4;
    //public Upgrade upgradePanel;
    //public Bonus bonusPanel;
    public bool isUpgradePanel = false;
    public bool isStart = false;
    public bool isFinish = false;
    public bool isMove = true;
    public float speed;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity = 0.1f;
    public Rigidbody rb;
    private float cameraAngleY;
    private float cameraAngleSpeed = 0.1f;
    private float cameraPosY;
    private float cameraPosSpeed = 0.1f;
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    public bool pressed = false;
    private Animator Anim;
    private CapsuleCollider capCol;
    private bool isInPool;
    public GameObject moneyPrefab;
    public float timeToFire = 0f;
    public float timeToFill = 0f;
    private RaycastHit hit;
    public GameObject waterSplash;
    public Image waterSlider;
    public float waterAmount = 0.5f;
    public float targetAngle2;
    public Image redWaterAlarm;
    public bool isRedAlarm = false;
    private float waterFillValue = 1.5f;
    public float waterDischargeValue = 0.2f;
    public AudioSource audioSource;
    public bool sTimer1 = true;
    public bool sTimer2 = true;
    //private GameManager _gm;

    public bool isMoneyStack = false;
    public List<GameObject> moneyList;
    public GameObject stackParent;
    public int stackNumber = 0;
    public int maxStackNumber = 10;
    public float stackSpeed = 0.1f;
    public GameObject maxText;

    public bool isDrink = false;
    public List<GameObject> drinkList;
    public GameObject drinkStackParent;
    public int drinkStackNumber = 0;
    public int maxDrinkStackNumber = 12;
    public float drinkStackSpeed = 0.1f;
    public GameObject tray;

  //  public List<GameObject> kStack1List;
  //  public List<GameObject> kStack2List;
 //   public List<GameObject> kStack3List;
 //   public int kStack1Number = 0;
  //  public int kStack2Number = 0;
  //  public int kStack3Number = 0;
    public bool sTimer3 = true;
    public bool sTimer4 = true;
    public bool sTimer5 = true;
    public bool sTimer10 = true;


   // public GameObject kFishStack;
 //   public GameObject kRiceStack;
   // public GameObject kSeaweedStack;

    public AudioSource m_AudioSource1;
    public AudioSource m_AudioSource2;
    public AudioSource m_AudioSource3;

    public GameObject moneyPay;
    public GameObject moneyStack;
    void Start()
    {
       // m_AudioSource = GetComponent<AudioSource>();
        //_gm = FindObjectOfType<GameManager>();
        Anim = gameObject.GetComponent<Animator>();
        capCol = gameObject.GetComponent<CapsuleCollider>();
       // moneyPay.SetActive(false);
    }

    /*
    public void HeroLoad(int moneyAmount) {

        
        Vector3 scale1 = moneyStack.transform.localScale;
        scale1.y = scale1.y + (2*moneyAmount);
        moneyStack.transform.localScale = scale1;
        moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BaseMap", new Vector2(1, moneyAmount));
        moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BumpMap", new Vector2(1, moneyAmount));
        if (moneyAmount <= 0) { moneyStack.SetActive(false); }


        for (int j = 0; j < moneyAmount; j++)
        {
            GameObject money1 = Instantiate(moneyPrefab, stackParent.transform);
            moneyList.Add(money1);
            float yukseklik = stackNumber * 0.1f;
            stackNumber++;
            money1.transform.localPosition = new Vector3(-0.1f, yukseklik, 0);
            money1.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }
    */
    //public void LevelStart()
    //{
    //    int secim1 = PlayerPrefs.GetInt("ShopSecim1");

    //    float oran = (Mathf.Pow(1.1f, secim1)) - 1;
    //    //Debug.Log("oran:"+oran);
    //    waterDischargeValue = waterDischargeValue - (waterDischargeValue * oran);
    //}
    #region yorum
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TutorialTarget" && _gm.isTutorial==true)
        {
            _gm.target.SetActive(false);
            _gm.Tutorial();

        }
        if (other.gameObject.tag == "Indoor")
        {
            Vcam1.Priority = 20;
            Vcam2.Priority = 5;
        }
        if (other.gameObject.tag == "WheelFortune")
        {
            Vcam3.Priority = 20;
            Vcam2.Priority = 5;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Upgrade" && isUpgradePanel == true)
        {
            isUpgradePanel = false;

        }
        if (other.gameObject.tag == "Bonus" && isUpgradePanel == true)
        {
            isUpgradePanel = false;

        }
        if (other.gameObject.tag == "Indoor")
        {
            Vcam1.Priority = 5;
            Vcam2.Priority = 20;
        }
        if (other.gameObject.tag == "WheelFortune")
        {
            Vcam2.Priority = 20;
            Vcam3.Priority = 5;
        }
    }

    */
    #endregion
    private void OnTriggerStay(Collider other)
    {
        #region yorum
        /*
        if (other.gameObject.tag == "Upgrade" && isUpgradePanel==false && pressed == false)
        {
            isMove = false;
            isUpgradePanel = true;
            //upgradePanel.OpenButton();
        }
        if (other.gameObject.tag == "Bonus" && isUpgradePanel == false && pressed == false)
        {
            isMove = false;
            isUpgradePanel = true;
            //bonusPanel.OpenButton();
        }
        */
        if (other.tag == "Deposit")
        {
            if (_gm.MoneyAmount > 0 && _gm.BodyMoney > 0 && stackNumber > 0 && sTimer2 == true && pressed == false)
            {
                sTimer2 = false;
                stackNumber--;
                GameObject money1 = Instantiate(moneyPrefab, stackParent.transform);
                LeanTween.cancel(money1);
                moneyList.Remove(money1);
                money1.transform.parent = other.transform;
                _gm.MoneyUpdate(-20);


                LeanTween.delayedCall(gameObject, 0.05f, () => { sTimer2 = true; });
                LeanTween.moveLocal(money1, new Vector3(money1.transform.localPosition.x / 2, 3, money1.transform.localPosition.z / 2), stackSpeed / 2f);
                LeanTween.moveLocal(money1, new Vector3(0, 0.1f, 0), stackSpeed).setOnComplete(() =>
                {
                    money1.transform.localPosition = new Vector3(0, 0.1f, 0);
                    money1.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    Destroy(money1, 0.1f);
                    //_gm.BodyMoney = _gm.BodyMoney - 20;
                    _gm.SafeMoney = _gm.SafeMoney + 20;
                    _gm.MoneyAmount = _gm.MoneyAmount + 20;
                   _gm. safeMoneyText.text = "$ " + ((int)_gm. SafeMoney).ToString();
                }).setDelay(stackSpeed / 2f);

            }
        }

        
        #endregion

        if (other.tag == "MoneySpawn" && stackNumber >= maxStackNumber && maxText.activeSelf == false)
        {
            maxText.SetActive(true);
            LeanTween.delayedCall(gameObject, 0.5f, () => { maxText.SetActive(false); });

        }

        if (other.tag == "MoneySpawn" && sTimer1 == true && stackNumber < maxStackNumber)// && pressed == false)
        {

            int moneyc = other.GetComponent<MoneySpawn>().moneyList.Count;
            if (moneyc > 0)
            {
                //m_AudioSource2.Play();
                
                sTimer1 = false;
                GameObject money1 = other.GetComponent<MoneySpawn>().moneyList[moneyc - 1];
                LeanTween.cancel(money1);
                other.GetComponent<MoneySpawn>().moneyNum--;
                other.GetComponent<MoneySpawn>().moneyList.Remove(money1);
                money1.transform.parent = stackParent.transform;

                LeanTween.delayedCall(gameObject, 0.01f, () => { sTimer1 = true; });

                if (!isMoneyStack)
                {
                    LeanTween.moveLocal(money1, new Vector3(0, 0.5f, 0), 0.05f).setOnComplete(() =>
                    {
                        money1.transform.localPosition = new Vector3(-0.1f, 0, 0);
                        money1.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        Destroy(money1, 0.05f);
                        _gm.MoneyUpdate(20);
                    });
                }
                else
                {
                    int kNumber = stackNumber;
                    stackNumber++;
                    if (stackNumber > 0) { moneyStack.SetActive(true); }

                    LeanTween.moveLocal(money1, new Vector3(0, (kNumber - 1) * 0.15f, 0), stackSpeed).setOnComplete(() =>
                    {
                    
                        Vector3 scale1 = moneyStack.transform.localScale;
                        scale1.y = scale1.y + 2;
                        moneyStack.transform.localScale = scale1;
                        moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BaseMap", new Vector2(1, moneyStack.GetComponent<MeshRenderer>().materials[1].GetTextureScale("_BaseMap").y + 1));
                        moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BumpMap", new Vector2(1, moneyStack.GetComponent<MeshRenderer>().materials[1].GetTextureScale("_BumpMap").y + 1));
                        LeanTween.cancel(money1);
                        Destroy(money1, 0.1f);
                        _gm.MoneyUpdate(20);
                    });
                }

            }
        }
        #region yorum
        /*
        if (other.tag == "DrinkPlace" && drinkStackNumber > 0 && other.transform.parent.parent.GetComponent<Table>().isCustomer==true) // Soft Drink dağıtma
        {

            bool isDrinkAvailable = false;
            int drinkRow = 0;
            bool isFastFoodAvailable = false;
            int fastFoodRow = 0;

            for (int j = drinkList.Count - 1; j > -1; j--)
            {
                if (drinkList[j].transform.tag == "Drink" && isDrinkAvailable == false) { isDrinkAvailable = true; drinkRow = j; }
            }

            if (other.transform.parent.parent.GetComponent<Table>().isDrink == false && isDrinkAvailable == true)
            {
                other.transform.parent.parent.GetComponent<Table>().DrinkStart();
                drinkStackNumber--;
                GameObject drink1 = drinkList[drinkRow];
                LeanTween.cancel(drink1);
                drinkList.Remove(drink1);
                Destroy(drink1);
                //other.transform.parent.parent.GetComponent<Table>().SpawnDrinkMoney();
            }
            for (int j = drinkList.Count - 1; j > -1; j--)
            {
                if (drinkList[j].transform.tag == "Hamburger" && isFastFoodAvailable == false) { isFastFoodAvailable = true; fastFoodRow = j; }
            }
            if (other.transform.parent.parent.GetComponent<Table>().isFastFood == false && isFastFoodAvailable == true)
            {
                other.transform.parent.parent.GetComponent<Table>().FastFoodStart();
                drinkStackNumber--;
                GameObject drink1 = drinkList[fastFoodRow];
                LeanTween.cancel(drink1);
                drinkList.Remove(drink1);
                Destroy(drink1);
                //other.transform.parent.parent.GetComponent<Table>().SpawnDrinkMoney();

            }

            
            for (int j = 0; j < drinkList.Count; j++)
            {
                float drinkRow2 = j / 2;
                if (j % 2 == 0) { drinkList[j].transform.localPosition = new Vector3(-0.25f, drinkRow2 / 4f, 0); }
                else { drinkList[j].transform.localPosition = new Vector3(0.25f, drinkRow2 / 4f, 0); }
            }

            if (drinkList.Count < 1) { tray.SetActive(false); Anim.SetBool("carry",false); }
            
        }

        if (other.tag == "KitchenArea" && pressed == false && sTimer4 == true)
        {

            //Debug.Log("malzeme
            if (drinkStackNumber > 0 && drinkList[drinkStackNumber - 1].GetComponent<Malzeme>())
            {
                if (!m_AudioSource3.isPlaying) m_AudioSource3.Play();
                sTimer4 = false;
                GameObject malzeme = drinkList[drinkStackNumber - 1];
                if (malzeme.GetComponent<Malzeme>().tip < 4)
                {                 
                    drinkList.Remove(malzeme);
                    drinkStackNumber--;
                }
                LeanTween.delayedCall(gameObject, 0.05f, () =>
                {
                    sTimer4 = true;
                });


                if (malzeme.GetComponent<Malzeme>().tip == 1)
                {
                    malzeme.transform.parent = other.GetComponent<KitchenArea>().kFishStack.transform;
                    other.GetComponent<KitchenArea>().kStack1List.Add(malzeme);
                    float yukseklik = other.GetComponent<KitchenArea>().kStack1Number * 0.1f;
                    int kNumber = other.GetComponent<KitchenArea>().kStack1Number;
                    other.GetComponent<KitchenArea>().kStack1Number++;
                    LeanTween.moveLocal(malzeme, new Vector3(0, (kNumber - 1) * 0.1f, 0), stackSpeed).setOnComplete(() =>
                    {
                        if (kNumber % 2 == 0) { malzeme.transform.localPosition = new Vector3(-0.1f, yukseklik, 0); }
                        else { malzeme.transform.localPosition = new Vector3(-0.1f, yukseklik, -0.33f); }
                        malzeme.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    });
                }
                if (malzeme.GetComponent<Malzeme>().tip == 2)
                {
                    malzeme.transform.parent = other.GetComponent<KitchenArea>().kRiceStack.transform;
                    other.GetComponent<KitchenArea>().kStack2List.Add(malzeme);
                    float yukseklik = other.GetComponent<KitchenArea>().kStack2Number * 0.1f;
                    int kNumber = other.GetComponent<KitchenArea>().kStack2Number;
                    other.GetComponent<KitchenArea>().kStack2Number++;
                    LeanTween.moveLocal(malzeme, new Vector3(0, (kNumber - 1) * 0.1f, 0), stackSpeed).setOnComplete(() =>
                    {
                        if (kNumber % 2 == 0) { malzeme.transform.localPosition = new Vector3(-0.1f, yukseklik, 0); }
                        else { malzeme.transform.localPosition = new Vector3(-0.1f, yukseklik, -0.33f); }
                        malzeme.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    });

                }
                if (malzeme.GetComponent<Malzeme>().tip == 3)
                {
                    malzeme.transform.parent = other.GetComponent<KitchenArea>().kSeaweedStack.transform;
                    other.GetComponent<KitchenArea>().kStack3List.Add(malzeme);
                    float yukseklik = other.GetComponent<KitchenArea>().kStack3Number * 0.1f;
                    int kNumber = other.GetComponent<KitchenArea>().kStack3Number;
                    other.GetComponent<KitchenArea>().kStack3Number++;
                    LeanTween.moveLocal(malzeme, new Vector3(0, (kNumber - 1) * 0.1f, 0), stackSpeed).setOnComplete(() =>
                    {
                        if (kNumber % 2 == 0) { malzeme.transform.localPosition = new Vector3(-0.1f, yukseklik, 0); }
                        else { malzeme.transform.localPosition = new Vector3(-0.1f, yukseklik, -0.33f); }
                        malzeme.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    });

                }
              

            }
            if (drinkList.Count < 1) { tray.SetActive(false); Anim.SetBool("carry", false); }
        }
        if (other.tag == "SushiSpawn" && sTimer2 == true && drinkStackNumber < maxDrinkStackNumber && pressed == false)
        {

            int drinkc = other.GetComponent<SushiSpawn>().FishList.Count;
            if (drinkc > 0)
            {
                sTimer2 = false;
                GameObject drink1 = other.GetComponent<SushiSpawn>().FishList[drinkc - 1];
                LeanTween.cancel(drink1);
                other.GetComponent<SushiSpawn>().fishNum--;
                other.GetComponent<SushiSpawn>().FishList.Remove(drink1);
                drink1.transform.parent = drinkStackParent.transform;
                //LeanTween.delayedCall(gameObject, 0.05f, () => {  sTimer2 = true; });
                drinkList.Add(drink1);
                if (drinkList.Count == 1) { tray.SetActive(true); Anim.SetBool("carry", true); }

                float drinkRow = drinkStackNumber / 2;


                LeanTween.moveLocal(drink1, new Vector3(drink1.transform.localPosition.x / 2, 3, drink1.transform.localPosition.z / 2), drinkStackSpeed / 2f);
                LeanTween.moveLocal(drink1, new Vector3(0, drinkRow / 4f, 0), drinkStackSpeed / 2f).setOnComplete(() =>
                {
                    drink1.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    if ((drinkStackNumber) % 2 == 0) { drink1.transform.localPosition = new Vector3(-0.25f, drinkRow / 4f, 0); }
                    else { drink1.transform.localPosition = new Vector3(0.25f, drinkRow / 4f, 0); }
                    drinkStackNumber++;
                    sTimer2 = true;

                }).setDelay(drinkStackSpeed / 2f);
            }
        }
        if (other.tag == "DrinkSpawn" && sTimer2 == true && drinkStackNumber < maxDrinkStackNumber)// && pressed == false)
        {

            int drinkc = other.GetComponent<MoneySpawn>().moneyList.Count;
            if (drinkc > 0)
            {
                
                sTimer2 = false;

                GameObject drink1 = other.GetComponent<MoneySpawn>().moneyList[drinkc - 1];
                LeanTween.cancel(drink1);
                other.GetComponent<MoneySpawn>().moneyNum--;
                other.GetComponent<MoneySpawn>().moneyList.Remove(drink1);
                drink1.transform.parent = drinkStackParent.transform;
                //LeanTween.delayedCall(gameObject, 0.05f, () => {  sTimer2 = true; });
                drinkList.Add(drink1);
                if (drinkList.Count == 1) { tray.SetActive(true); Anim.SetBool("carry", true); }

                float drinkRow = drinkStackNumber / 2;

                LeanTween.delayedCall(gameObject, 0.05f, () => { sTimer2 = true; });
                LeanTween.moveLocal(drink1, new Vector3(drink1.transform.localPosition.x / 2, 3, drink1.transform.localPosition.z / 2), drinkStackSpeed / 2f);
                LeanTween.moveLocal(drink1, new Vector3(0, drinkRow / 2, 0), drinkStackSpeed / 2f).setOnComplete(() =>
                {
                    drink1.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    if ((drinkStackNumber) % 2 == 0) { drink1.transform.localPosition = new Vector3(-0.25f, drinkRow / 2.2f, 0); }
                    else { drink1.transform.localPosition = new Vector3(0.25f, drinkRow / 2.2f, 0); }
                    drinkStackNumber++;
                               

                }).setDelay(drinkStackSpeed / 2f);
            }
        }
        if (other.tag == "FishArea" && sTimer2 == true && drinkStackNumber < maxDrinkStackNumber)// && pressed == false)
        {
           // Debug.Log("Deneme");
            int drinkc = other.GetComponent<FishSpawn>().FishList.Count;
            if (drinkc > 0)
            {

                sTimer2 = false;
                GameObject drink1 = other.GetComponent<FishSpawn>().FishList[drinkc - 1];
                LeanTween.cancel(drink1);
                other.GetComponent<FishSpawn>().fishNum--;
                other.GetComponent<FishSpawn>().FishList.Remove(drink1);
                drink1.transform.parent = drinkStackParent.transform;
                //LeanTween.delayedCall(gameObject, 0.05f, () => {  sTimer2 = true; });
                drinkList.Add(drink1);
                if (drinkList.Count == 1) { tray.SetActive(true); Anim.SetBool("carry", true); }

                float drinkRow = drinkStackNumber / 2;


               // LeanTween.moveLocal(drink1, new Vector3(drink1.transform.localPosition.x / 2, 2, drink1.transform.localPosition.z / 2), drinkStackSpeed / 2f);
                LeanTween.moveLocal(drink1, new Vector3(0, drinkRow / 4, 0), drinkStackSpeed).setOnComplete(() =>
                {
                    drink1.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (drink1.GetComponent<Malzeme>().tip > 1) {
                       drink1.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    }
                    if ((drinkStackNumber) % 2 == 0) { drink1.transform.localPosition = new Vector3(-0.25f, drinkRow / 4f, 0); }
                    else { drink1.transform.localPosition = new Vector3(0.25f, drinkRow / 4f, 0); }
                    drinkStackNumber++;
                    sTimer2 = true;

                });
            }
        }
        if (drinkList.Count <= 0) { tray.SetActive(false);}

        */
        #endregion
    }




    public void MoneyDecrease(float BodyMoney)
    {
        stackNumber = (int)Mathf.Floor(BodyMoney / 20);
        Vector3 scale1 = moneyStack.transform.localScale;
        scale1.y = (2 * Mathf.Floor(BodyMoney / 20));
        moneyStack.transform.localScale = scale1;
        moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BaseMap", new Vector2(1, Mathf.Floor(BodyMoney / 20)));
        moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BumpMap", new Vector2(1, Mathf.Floor(BodyMoney / 20)));

        if (BodyMoney <= 0) { moneyStack.SetActive(false); }


       
    }



    public void Update()
    {
        if (isStart && !isFinish && isMove)
        {
            Swipe();
        }
    }

    public void Swipe()
    {

        if (this.transform.position.y < -3)
        {
            this.transform.position = this.transform.position + new Vector3(0, 6, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            pressed = true;
            //Vcam4.Priority = 12;

            if (!isInPool) { Anim.SetBool("walk", true); m_AudioSource1.Play(); }
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            pressed = false;
            //Vcam4.Priority = 8;

            if (!isInPool) { Anim.SetBool("walk", false); m_AudioSource1.Pause(); }
        }



        if (pressed == true)
        {
            secondPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            currentSwipe.Normalize();


            Vector3 direction = new Vector3(currentSwipe.x, 0, currentSwipe.y);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


            transform.rotation = newRot;
            transform.position = transform.position + (transform.forward * speed * Time.deltaTime);
            Vector3 pos1 = transform.position;
            pos1.y = Mathf.Clamp(pos1.y, -1, 2);
            transform.position = pos1;
        }

    
    }
}