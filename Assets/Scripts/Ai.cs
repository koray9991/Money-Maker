using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Ai : MonoBehaviour
{
    public float rotateSpeed;
    public float moveSpeed;
    public Animator anim;
    public float stackedMoneyCount;
    public float totalMoneyCount, bankMoneyCount;   
    public Transform stackTransform;
    public int maxStackCount;   
    public Transform Bank;
    public Text bankMoneyText;
    public GameObject targetBuilding;

    [HideInInspector] public int decisionMoveDirection;
    [HideInInspector] public float reduceBankMoneyTimer;
    public List<GameObject> buildingAreas;
    [HideInInspector] public int targetStackCount;
    [HideInInspector] public List<GameObject> stackedMoneys;
    [HideInInspector] public bool moveBank;
    [HideInInspector] public bool moveMoney;
    [HideInInspector] public bool moveBuilding;
    [HideInInspector] public bool moneyThrowBool;
    [HideInInspector] public float throwMoneyTimer;
    [HideInInspector] public bool moving;
    [HideInInspector] public bool fall;
    [HideInInspector] public float fallTimer;
    public List<GameObject> buyableAreas;
    public List<GameObject> cheapestBuildings;
    public GameMngr gm;

    public GameObject[] bankStack;
    public int bankStackMode;
    public float moneyScaleChange;

    void Start()
    {
        targetStackCount = maxStackCount - (Random.Range(0, 10));
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("BuildingPlane"))
        {

            buildingAreas.Add(gameObject);
        }

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMngr>();

       
        moveMoney = true;
        moving = true;
       
    }

    static int SortByCost(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<BuildingPlane>().cost.CompareTo(p2.GetComponent<BuildingPlane>().cost);
    }
    static int SortByBuyCost(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<BuildingPlane>().startCost.CompareTo(p2.GetComponent<BuildingPlane>().startCost);
    }
    void FixedUpdate()
    {
       
        if (fall)
        {
            if (stackedMoneyCount > 0)
            {
                for (int i = 0; i < stackedMoneyCount; i++)
                {
                    
                    stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(transform.position + new Vector3(Random.Range(-5f, 5f), 0.35f, Random.Range(-5f, 5f)), 2, 1, 0.5f);
                    stackedMoneyCount -= 1;
                    stackedMoneys[stackTransform.childCount - 1].GetComponent<Money>().StartCoroutine(stackedMoneys[stackTransform.childCount - 1].GetComponent<Money>().Tag());
                    totalMoneyCount -= 1;
                 //   stackedMoneys[stackTransform.childCount - 1].transform.tag = "Money";
                    stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                 //   Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2);
                    stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                }
            }
            GetComponent<CapsuleCollider>().isTrigger = true;
           
            fallTimer += Time.deltaTime;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
            {
                
                anim.SetBool("fall", false);
                anim.SetBool("walk", true);

            }
            if (fallTimer > 1.75f)
            {
                fall = false;
                fallTimer = 0;
                GetComponent<CapsuleCollider>().isTrigger = false;
            }
        }
        else
        {
            if (moveMoney)
            {
                MoveClosestTarget();
            }
            if (moveBuilding && targetBuilding!=null)
            {
                MoveBuilding();
            }
            if (moveBank)
            {
                MoveBank();
            }
        }

        if (moneyThrowBool)
        {
            throwMoneyTimer += Time.deltaTime;
        }
       
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (bankMoneyCount == 0)
        {
           
            for (int i = 0; i < bankStack.Length; i++)
            {
               bankStack[i].transform.localScale = new Vector3(bankStack[i].transform.localScale.x, 0, bankStack[i].transform.localScale.z);

            }
            bankStackMode = 0;
        }
    }
   public void MoveClosestTarget()
    {
     
        float distanceClosestTarget = Mathf.Infinity;
        GameObject closestTarget = null;
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Money");
        if (allTargets.Length != 0)
        {
            foreach (GameObject currentTarget in allTargets)
            {
                float distanceToTarget = (currentTarget.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToTarget < distanceClosestTarget)
                {
                    distanceClosestTarget = distanceToTarget;
                    closestTarget = currentTarget;
                }
            }
           
            transform.position = Vector3.MoveTowards(transform.position, closestTarget.transform.position, moveSpeed);
            var targetRotation = Quaternion.LookRotation(closestTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            if (cheapestBuildings.Count>0)
            {
                for (int i = cheapestBuildings.Count - 1; i >= 0; i--)
                {
                    cheapestBuildings.Remove(cheapestBuildings[i]);
                }
            }
            if (targetRotation == Quaternion.Euler(0, 0, 0))
            {
                moveMoney = false;
                decisionMoveDirection = Random.Range(1, 3);
                if (decisionMoveDirection == 1)
                {
                    targetBuilding = null;
                    //moveBuilding = true;
                    //buildingAreas.Sort(SortByCost);
                    //targetBuilding = buildingAreas[Random.Range(0, 3)];
                    //GetComponent<CapsuleCollider>().isTrigger = false;

                    moveBuilding = true;
                    buildingAreas.Sort(SortByCost);
                    if (buildingAreas.Count >= 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            cheapestBuildings.Add(buildingAreas[i]);
                        }
                    }
                    if (buildingAreas.Count == 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            cheapestBuildings.Add(buildingAreas[i]);
                        }
                    }
                    if (buildingAreas.Count == 1)
                    {

                        cheapestBuildings.Add(buildingAreas[0]);

                    }
                    if (buyableAreas.Count >= 2)
                    {
                        buyableAreas.Sort(SortByBuyCost);
                        if (buyableAreas[0].GetComponent<BuildingPlane>().startCost <= totalMoneyCount)
                        {
                            cheapestBuildings.Add(buyableAreas[0]);
                        }
                        if (buyableAreas[1].GetComponent<BuildingPlane>().startCost <= totalMoneyCount)
                        {
                            cheapestBuildings.Add(buyableAreas[1]);
                        }

                    }
                    if (cheapestBuildings.Count > 0)
                    {
                        targetBuilding = cheapestBuildings[Random.Range(0, cheapestBuildings.Count - 1)];
                    }
                    else
                    {
                        decisionMoveDirection = 2;
                        
                            moveBank = true;
                            GetComponent<CapsuleCollider>().isTrigger = false;
                            targetBuilding = null;
                        
                    }
                    GetComponent<CapsuleCollider>().isTrigger = false;


                }
                if (decisionMoveDirection == 2)
                {
                    moveBank = true;
                    GetComponent<CapsuleCollider>().isTrigger = false;
                    targetBuilding = null;
                }
            }
        }

       
        if (stackedMoneyCount >= targetStackCount && moveMoney)
        {
            moveMoney = false;
            decisionMoveDirection = Random.Range(1, 3);
            if (decisionMoveDirection == 1)
            {

                targetBuilding = null;
                moveBuilding = true;
                buildingAreas.Sort(SortByCost);
                if (buildingAreas.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cheapestBuildings.Add(buildingAreas[i]);
                    }
                }
                if (buildingAreas.Count == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        cheapestBuildings.Add(buildingAreas[i]);
                    }
                }
                if (buildingAreas.Count == 1)
                {

                    cheapestBuildings.Add(buildingAreas[0]);

                }
                if (buyableAreas.Count >= 2)
                {
                    buyableAreas.Sort(SortByBuyCost);
                    if (buyableAreas[0].GetComponent<BuildingPlane>().startCost <= totalMoneyCount)
                    {
                        cheapestBuildings.Add(buyableAreas[0]);
                    }
                    if (buyableAreas[1].GetComponent<BuildingPlane>().startCost <= totalMoneyCount)
                    {
                        cheapestBuildings.Add(buyableAreas[1]);
                    }

                }
                if (cheapestBuildings.Count > 0)
                {
                    targetBuilding = cheapestBuildings[Random.Range(0, cheapestBuildings.Count - 1)];
                }
                else
                {
                    decisionMoveDirection = 2;

                    moveBank = true;
                    GetComponent<CapsuleCollider>().isTrigger = false;
                    targetBuilding = null;

                }
                GetComponent<CapsuleCollider>().isTrigger = false;


            }
            if (decisionMoveDirection == 2)
            {
                moveBank = true;
                GetComponent<CapsuleCollider>().isTrigger = false;
                targetBuilding = null;
            }

        }
    }
  public  void MoveBank()
    {
        var targetRotation = Quaternion.LookRotation(Bank.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        if (cheapestBuildings.Count > 0)
        {
            for (int i = cheapestBuildings.Count - 1; i >= 0; i--)
            {
                cheapestBuildings.Remove(cheapestBuildings[i]);
            }
        }
        if (Vector3.Distance(transform.position, Bank.position) > 1 )
        {
            transform.position = Vector3.MoveTowards(transform.position, Bank.position, moveSpeed);
            
        }
        else
        {
            
            if (stackedMoneyCount > 0 )
            {
                GetComponent<Rigidbody>().isKinematic = true;
                moving = false;
                anim.SetBool("idle", true);
                anim.SetBool("walk", false);

               
            }
            else
            {
                moveBank = false;
                decisionMoveDirection = Random.Range(0, 2);
                if (decisionMoveDirection == 0)
                {
                    moveMoney = true;
                    GetComponent<CapsuleCollider>().isTrigger = false;
                    targetBuilding = null;
                }
                if (decisionMoveDirection == 1)
                {
                    //moveBuilding = true;
                    //buildingAreas.Sort(SortByCost);
                    //targetBuilding = buildingAreas[Random.Range(0, 3)];
                    //GetComponent<CapsuleCollider>().isTrigger = false;
                    targetBuilding = null;
                    moveBuilding = true;
                    buildingAreas.Sort(SortByCost);
                    if (buildingAreas.Count >= 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            cheapestBuildings.Add(buildingAreas[i]);
                        }
                    }
                    if(buildingAreas.Count == 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            cheapestBuildings.Add(buildingAreas[i]);
                        }
                    }
                    if (buildingAreas.Count == 1)
                    {
                       
                            cheapestBuildings.Add(buildingAreas[0]);
                        
                    }
                    if (buyableAreas.Count >= 2)
                    {
                        buyableAreas.Sort(SortByBuyCost);
                        if (buyableAreas[0].GetComponent<BuildingPlane>().startCost <= totalMoneyCount)
                        {
                            cheapestBuildings.Add(buyableAreas[0]);
                        }
                        if (buyableAreas[1].GetComponent<BuildingPlane>().startCost <= totalMoneyCount)
                        {
                            cheapestBuildings.Add(buyableAreas[1]);
                        }
                       
                    }
                    if (cheapestBuildings.Count > 0)
                    {
                        targetBuilding = cheapestBuildings[Random.Range(0, cheapestBuildings.Count - 1)];
                    }
                    else
                    {
                        decisionMoveDirection = 0;

                        moveMoney = true;
                        GetComponent<CapsuleCollider>().isTrigger = false;
                        targetBuilding = null;
                    }
                    
                    
                    GetComponent<CapsuleCollider>().isTrigger = false;


                }
                anim.SetBool("walk", true);
                anim.SetBool("idle", false);
                targetStackCount = maxStackCount - (Random.Range(0, 10));
                moving = true;
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        
       
    }
   public void MoveBuilding()
    {
       
        var targetRotation = Quaternion.LookRotation(targetBuilding.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetBuilding.transform.position) > 1 && totalMoneyCount > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetBuilding.transform.position, moveSpeed);
        }
        else
        {

            if (totalMoneyCount > 0)
            {
                anim.SetBool("idle", true);
                anim.SetBool("walk", false);
                moving = false;
                GetComponent<Rigidbody>().isKinematic = true;

            }
            else
            {
                targetBuilding = null;
                decisionMoveDirection = 0;
                moveBuilding = false;
                moveMoney = true;
                GetComponent<CapsuleCollider>().isTrigger = false;
                anim.SetBool("walk", true);
                anim.SetBool("idle", false);
                targetStackCount = maxStackCount - (Random.Range(0, 10));
                moving = true;
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Money")
        {
            if (stackedMoneyCount <= maxStackCount)
            {
                stackedMoneyCount += 1;
                totalMoneyCount += 1;


                stackedMoneys.Add(other.gameObject);
                other.tag = "StackedMoney";
                other.transform.parent = stackTransform;
                other.transform.DOLocalMove(new Vector3(0, stackedMoneyCount, 0), 0.3f);
                // other.transform.DOLocalJump(new Vector3(0, stackedMoneyCount/2, 0), 10, 1, 1);
                other.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                other.transform.GetChild(0).gameObject.SetActive(true);
                other.transform.GetChild(1).gameObject.SetActive(true);
                other.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
                other.transform.GetChild(1).GetComponent<TrailRenderer>().enabled = true;
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!moving)
        {
            if (other.tag == "MoneyBox")
            {

                if (stackedMoneyCount > 0 )
                {

                    moneyThrowBool = true;

                    if (throwMoneyTimer > 0.05f)
                    {
                        GetComponent<CapsuleCollider>().isTrigger = true;
                        throwMoneyTimer = 0;
                     //   other.transform.GetChild(0).transform.DOLocalRotateQuaternion(Quaternion.Euler(-90, -90, 0), 0.2f);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position + new Vector3(0, -2f, 0), 3, 1, 1);
                        // stackTransform.GetChild(stackTransform.childCount - 1).transform.DOMove(other.transform.position, 0.1f);
                        stackedMoneyCount -= 1;
                        bankMoneyCount += 1;
                        bankMoneyText.text = "$ " + bankMoneyCount;
                        bankStack[bankStackMode].transform.localScale += new Vector3(0, moneyScaleChange, 0);

                        bankStackMode += 1;
                        if (bankStackMode == 9) { bankStackMode = 0; }
                        stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                        Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2f);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                    }

                }


            }
            if (other.tag == "BuildingPlane")
            {
                if (transform.tag == "PlayerRed" && (other.GetComponent<BuildingPlane>().color == "red" || other.GetComponent<BuildingPlane>().color == "") && !gm.redDestroyed )
                {
                
                        if (stackedMoneyCount > 0)
                        {
                            moneyThrowBool = true;

                            if (throwMoneyTimer > 0.02f)
                            {

                                throwMoneyTimer = 0;
                                // stackTransform.GetChild(stackTransform.childCount - 1).transform.DOMove(other.transform.position, 0.1f);
                                stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position + new Vector3(0, -3, 0), 5, 1, 1);
                                GetComponent<CapsuleCollider>().isTrigger = true;
                                stackedMoneyCount -= 1;

                                totalMoneyCount -= 1;

                                stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                                Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 0.5f);
                                stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.red;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                           if( other.GetComponent<BuildingPlane>().color == "")
                            {
                                other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            }
                                
                                other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                                if (other.GetComponent<BuildingPlane>().cost == 0)
                                {
                                    if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                    {
                                        if (transform.tag == "PlayerRed")
                                        {
                                            if (other.GetComponent<BuildingPlane>().color != "red")
                                            {
                                                other.GetComponent<BuildingPlane>().color = "red";
                                                other.GetComponent<BuildingPlane>().ChangeColor();

                                            }
                                            if (other.GetComponent<BuildingPlane>().color == "red")
                                            {
                                                other.GetComponent<BuildingPlane>().CostUpdate();


                                            }
                                        }
                                        if (transform.tag == "PlayerGreen")
                                        {
                                            if (other.GetComponent<BuildingPlane>().color != "green")
                                            {
                                                other.GetComponent<BuildingPlane>().color = "green";
                                                other.GetComponent<BuildingPlane>().ChangeColor();

                                            }
                                            if (other.GetComponent<BuildingPlane>().color == "green")
                                            {
                                                other.GetComponent<BuildingPlane>().CostUpdate();


                                            }
                                        }
                                        if (transform.tag == "PlayerYellow")
                                        {
                                            if (other.GetComponent<BuildingPlane>().color != "yellow")
                                            {
                                                other.GetComponent<BuildingPlane>().color = "yellow";
                                                other.GetComponent<BuildingPlane>().ChangeColor();

                                            }
                                            if (other.GetComponent<BuildingPlane>().color == "yellow")
                                            {
                                                other.GetComponent<BuildingPlane>().CostUpdate();


                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (transform.tag == "PlayerRed")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "red";
                                            other.GetComponent<BuildingPlane>().CostUpdate();
                                        }
                                        if (transform.tag == "PlayerGreen")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "green";
                                            other.GetComponent<BuildingPlane>().CostUpdate();
                                        }
                                        if (transform.tag == "PlayerYellow")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "yellow";
                                            other.GetComponent<BuildingPlane>().CostUpdate();
                                        }

                                    }





                                }



                            }





                        }
                        if (stackedMoneyCount == 0 && bankMoneyCount > 0)
                        {
                            reduceBankMoneyTimer += Time.deltaTime;
                            if (reduceBankMoneyTimer >0.001f)
                            {
                                GetComponent<CapsuleCollider>().isTrigger = true;
                                reduceBankMoneyTimer = 0;
                                bankMoneyCount -= 1;
                                bankMoneyText.text = "$ " + bankMoneyCount;
                            bankStackMode -= 1;
                            if (bankStackMode == -1)
                            {
                                bankStackMode = 8;
                            }
                            bankStack[bankStackMode].transform.localScale += new Vector3(0, -moneyScaleChange, 0);
                            totalMoneyCount -= 1;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.red;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            if (other.GetComponent<BuildingPlane>().color == "")
                            {
                                other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            }
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                                if (other.GetComponent<BuildingPlane>().cost == 0)
                                {
                                    if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                    {
                                        if (transform.tag == "PlayerRed")
                                        {
                                            if (other.GetComponent<BuildingPlane>().color != "red")
                                            {
                                                other.GetComponent<BuildingPlane>().color = "red";
                                                other.GetComponent<BuildingPlane>().ChangeColor();

                                            }
                                            if (other.GetComponent<BuildingPlane>().color == "red")
                                            {
                                                other.GetComponent<BuildingPlane>().CostUpdate();


                                            }
                                        }
                                        if (transform.tag == "PlayerGreen")
                                        {
                                            if (other.GetComponent<BuildingPlane>().color != "green")
                                            {
                                                other.GetComponent<BuildingPlane>().color = "green";
                                                other.GetComponent<BuildingPlane>().ChangeColor();

                                            }
                                            if (other.GetComponent<BuildingPlane>().color == "green")
                                            {
                                                other.GetComponent<BuildingPlane>().CostUpdate();


                                            }
                                        }
                                        if (transform.tag == "PlayerYellow")
                                        {
                                            if (other.GetComponent<BuildingPlane>().color != "yellow")
                                            {
                                                other.GetComponent<BuildingPlane>().color = "yellow";
                                                other.GetComponent<BuildingPlane>().ChangeColor();

                                            }
                                            if (other.GetComponent<BuildingPlane>().color == "yellow")
                                            {
                                                other.GetComponent<BuildingPlane>().CostUpdate();


                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (transform.tag == "PlayerRed")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "red";
                                            other.GetComponent<BuildingPlane>().CostUpdate();
                                        }
                                        if (transform.tag == "PlayerGreen")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "green";
                                            other.GetComponent<BuildingPlane>().CostUpdate();
                                        }
                                        if (transform.tag == "PlayerYellow")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "yellow";
                                            other.GetComponent<BuildingPlane>().CostUpdate();
                                        }

                                    }





                                }
                            }
                        }
                    
                }
                else if (transform.tag == "PlayerYellow" && (other.GetComponent<BuildingPlane>().color == "yellow" || other.GetComponent<BuildingPlane>().color == "") && !gm.yellowDestroyed )
                {

                    if (stackedMoneyCount > 0)
                    {
                        moneyThrowBool = true;

                        if (throwMoneyTimer > 0.02f)
                        {

                            throwMoneyTimer = 0;
                            // stackTransform.GetChild(stackTransform.childCount - 1).transform.DOMove(other.transform.position, 0.1f);
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position + new Vector3(0, -3, 0), 5, 1, 1);
                            GetComponent<CapsuleCollider>().isTrigger = true;
                            stackedMoneyCount -= 1;

                            totalMoneyCount -= 1;

                            stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                            Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 0.5f);
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.yellow;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            if (other.GetComponent<BuildingPlane>().color == "")
                            {
                                other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            }
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost == 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "red")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "red";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "red")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "green")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "green";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "green")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "yellow";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }

                                }
                                else
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "red";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "green";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "yellow";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }

                                }





                            }



                        }





                    }
                    if (stackedMoneyCount == 0 && bankMoneyCount > 0)
                    {
                        reduceBankMoneyTimer += Time.deltaTime;
                        if (reduceBankMoneyTimer > 0.001f)
                        {
                            GetComponent<CapsuleCollider>().isTrigger = true;
                            reduceBankMoneyTimer = 0;
                            bankMoneyCount -= 1;
                            bankMoneyText.text = "$ " + bankMoneyCount;
                            bankStackMode -= 1;
                            if (bankStackMode == -1)
                            {
                                bankStackMode = 8;
                            }
                            bankStack[bankStackMode].transform.localScale += new Vector3(0, -moneyScaleChange, 0);
                            totalMoneyCount -= 1;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.yellow;
                            if (other.GetComponent<BuildingPlane>().color == "")
                            {
                                other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            }
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost == 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "red")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "red";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "red")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "green")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "green";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "green")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "yellow";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }

                                }
                                else
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "red";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "green";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "yellow";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }

                                }





                            }
                        }
                    }

                }
                else if (transform.tag == "PlayerGreen" && (other.GetComponent<BuildingPlane>().color == "green" || other.GetComponent<BuildingPlane>().color == "") && !gm.greenDestroyed)
                {

                    if (stackedMoneyCount > 0)
                    {
                        moneyThrowBool = true;

                        if (throwMoneyTimer > 0.02f)
                        {

                            throwMoneyTimer = 0;
                            // stackTransform.GetChild(stackTransform.childCount - 1).transform.DOMove(other.transform.position, 0.1f);
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position + new Vector3(0, -3, 0), 5, 1, 1);
                            GetComponent<CapsuleCollider>().isTrigger = true;
                            stackedMoneyCount -= 1;

                            totalMoneyCount -= 1;

                            stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                            Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 0.5f);
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.green;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            if (other.GetComponent<BuildingPlane>().color == "")
                            {
                                other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            }
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost == 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "red")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "red";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "red")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "green")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "green";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "green")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "yellow";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }

                                }
                                else
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "red";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "green";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "yellow";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }

                                }





                            }



                        }





                    }
                    if (stackedMoneyCount == 0 && bankMoneyCount > 0)
                    {
                        reduceBankMoneyTimer += Time.deltaTime;
                        if (reduceBankMoneyTimer > 0.001f)
                        {
                            GetComponent<CapsuleCollider>().isTrigger = true;
                            reduceBankMoneyTimer = 0;
                            bankMoneyCount -= 1;
                            bankMoneyText.text = "$ " + bankMoneyCount;
                            bankStackMode -= 1;
                            if (bankStackMode == -1)
                            {
                                bankStackMode = 8;
                            }
                            bankStack[bankStackMode].transform.localScale += new Vector3(0, -moneyScaleChange, 0);
                            totalMoneyCount -= 1;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.green;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            if (other.GetComponent<BuildingPlane>().color == "")
                            {
                                other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            }
                            
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost == 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "red")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "red";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "red")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "green")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "green";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "green")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        if (other.GetComponent<BuildingPlane>().color != "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().color = "yellow";
                                            other.GetComponent<BuildingPlane>().ChangeColor();

                                        }
                                        if (other.GetComponent<BuildingPlane>().color == "yellow")
                                        {
                                            other.GetComponent<BuildingPlane>().CostUpdate();


                                        }
                                    }

                                }
                                else
                                {
                                    if (transform.tag == "PlayerRed")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "red";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerGreen")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "green";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }
                                    if (transform.tag == "PlayerYellow")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "yellow";
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                    }

                                }





                            }
                        }
                    }

                }
                else
                {
                    if(bankMoneyCount>= other.GetComponent<BuildingPlane>().startCost)
                    {
                        bankMoneyCount -= other.GetComponent<BuildingPlane>().startCost;
                        bankMoneyText.text = "$ " + bankMoneyCount;
                        totalMoneyCount -= other.GetComponent<BuildingPlane>().startCost;
                        other.GetComponent<BuildingPlane>().cost = 0;
                        if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                        {
                            if (transform.tag == "PlayerRed")
                            {
                                if (other.GetComponent<BuildingPlane>().color != "red")
                                {
                                    other.GetComponent<BuildingPlane>().color = "red";
                                    other.GetComponent<BuildingPlane>().ChangeColor();
                                    for (int i = 0; i < other.GetComponent<BuildingPlane>().startCost; i++)
                                    {
                                        bankStackMode -= 1;
                                        if (bankStackMode == -1)
                                        {
                                            bankStackMode = 8;
                                        }
                                        bankStack[bankStackMode].transform.localScale += new Vector3(0, -moneyScaleChange, 0);
                                    }
                                }

                            }
                            if (transform.tag == "PlayerGreen")
                            {
                                if (other.GetComponent<BuildingPlane>().color != "green")
                                {
                                    other.GetComponent<BuildingPlane>().color = "green";
                                    other.GetComponent<BuildingPlane>().ChangeColor();
                                    for (int i = 0; i < other.GetComponent<BuildingPlane>().startCost; i++)
                                    {
                                        bankStackMode -= 1;
                                        if (bankStackMode == -1)
                                        {
                                            bankStackMode = 8;
                                        }
                                        bankStack[bankStackMode].transform.localScale += new Vector3(0, -moneyScaleChange, 0);
                                    }
                                }

                            }
                            if (transform.tag == "PlayerYellow")
                            {
                                if (other.GetComponent<BuildingPlane>().color != "yellow")
                                {
                                    other.GetComponent<BuildingPlane>().color = "yellow";
                                    other.GetComponent<BuildingPlane>().ChangeColor();
                                    for (int i = 0; i < other.GetComponent<BuildingPlane>().startCost; i++)
                                    {
                                        bankStackMode -= 1;
                                        if (bankStackMode == -1)
                                        {
                                            bankStackMode = 8;
                                        }
                                        bankStack[bankStackMode].transform.localScale += new Vector3(0, -moneyScaleChange, 0);
                                    }
                                }

                            }

                        }







                    }
                    else
                    {
                        moveBank = true;
                        moveBuilding = false;
                        GetComponent<CapsuleCollider>().isTrigger = false;
                        anim.SetBool("walk", true);
                        anim.SetBool("idle", false);
                        moving = true;
                    }
                }
                  

            }

           
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "PlayerBlue")
        {
            if (collision.gameObject.GetComponent<Character>().gm.stackedMoneyCount <= stackedMoneyCount && stackedMoneyCount != 0)
            {
                collision.gameObject.GetComponent<Character>().fall = true;
                collision.gameObject.GetComponent<Character>().anim.SetInteger("movement", 2);
            }
            


        }
        if (collision.gameObject.tag == "PlayerRed" || collision.gameObject.tag == "PlayerGreen" || collision.gameObject.tag == "PlayerYellow")
        {
            
            if (stackedMoneyCount >= collision.gameObject.GetComponent<Ai>().stackedMoneyCount && stackedMoneyCount !=0)
            {
                collision.gameObject.GetComponent<Ai>().fall = true;
                collision.gameObject.GetComponent<Ai>().anim.SetBool("fall", true);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MoneyBox")
        {
           
         //   other.transform.GetChild(0).transform.DOLocalRotateQuaternion(Quaternion.Euler(-90, 0, 0), 0.2f);
        }
    }
    
}
