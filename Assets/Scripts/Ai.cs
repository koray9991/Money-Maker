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
    [HideInInspector] public List<GameObject> buildingAreas;
    [HideInInspector] public int targetStackCount;
    [HideInInspector] public List<GameObject> stackedMoneys;
    [HideInInspector] public bool moveBank;
    [HideInInspector] public bool moveMoney;
    [HideInInspector] public bool moveBuilding;
    [HideInInspector] public bool moneyThrowBool;
    [HideInInspector] public float throwMoneyTimer;
    [HideInInspector] public bool moving;
    void Start()
    {
        targetStackCount = maxStackCount - (Random.Range(0, 10));
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("BuildingPlane"))
        {

            buildingAreas.Add(gameObject);
        }
        targetBuilding = buildingAreas[Random.Range(0, buildingAreas.Count)];
        moveMoney = true;
        moving = true;
       
    }

        // Update is called once per frame
        void Update()
    {               
        if (moveMoney)
        {
            MoveClosestTarget();
        }
        if (moveBuilding)
        {
            MoveBuilding();
        }
        if (moveBank)
        {
            MoveBank();
        }
        

        if (moneyThrowBool)
        {
            throwMoneyTimer += Time.deltaTime;
        }

    }
    void MoveClosestTarget()
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
        }


        if (stackedMoneyCount >= targetStackCount && moveMoney)
        {
            moveMoney = false;
            decisionMoveDirection = Random.Range(1, 3);
            if (decisionMoveDirection == 1)
            {
                moveBuilding = true;
            }
            if (decisionMoveDirection == 2)
            {
                moveBank = true;
            }

        }
    }
    void MoveBank()
    {
        var targetRotation = Quaternion.LookRotation(Bank.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, Bank.position) > 1 )
        {
            transform.position = Vector3.MoveTowards(transform.position, Bank.position, moveSpeed);
        }
        else
        {
            
            if (stackedMoneyCount > 0 )
            {
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
                }
                if (decisionMoveDirection == 1)
                {
                    moveBuilding = true;
                }
                anim.SetBool("walk", true);
                anim.SetBool("idle", false);
                targetStackCount = maxStackCount - (Random.Range(0, 10));
                moving = true;
            }
        }
        
       
    }
    void MoveBuilding()
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
                
            }
            else
            {
                decisionMoveDirection = 0;
                moveBuilding = false;
                moveMoney = true;
                anim.SetBool("walk", true);
                anim.SetBool("idle", false);
                targetStackCount = maxStackCount - (Random.Range(0, 10));
                moving = true;
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
                other.transform.DOLocalJump(new Vector3(0, stackedMoneyCount / 10, 0), 2, 1, 1);
                other.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
           
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!moving)
        {
            if (other.tag == "MoneyBoxBlue")
            {

                if (stackedMoneyCount > 0)
                {

                    moneyThrowBool = true;

                    if (throwMoneyTimer > 0.05f)
                    {
                        throwMoneyTimer = 0;
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position, 2, 1, 1);
                        stackedMoneyCount -= 1;
                        bankMoneyCount += 1;
                        bankMoneyText.text = "$ " + bankMoneyCount;
                        stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                        Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                    }

                }


            }
            if (other.tag == "BuildingPlane")
            {


                if (stackedMoneyCount > 0)
                {
                    moneyThrowBool = true;

                    if (throwMoneyTimer > 0.05f)
                    {

                        throwMoneyTimer = 0;
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position, 2, 1, 1);

                        stackedMoneyCount -= 1;

                        totalMoneyCount -= 1;

                        stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                        Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                        other.GetComponent<BuildingPlane>().cost -= 1;                        
                        other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                        other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                        if (other.GetComponent<BuildingPlane>().cost == 0)
                        {
                            if (transform.tag == "PlayerRed")
                            {
                                other.GetComponent<BuildingPlane>().blue = false;
                                other.GetComponent<BuildingPlane>().red = true;
                                other.GetComponent<BuildingPlane>().yellow = false;
                                other.GetComponent<BuildingPlane>().green = false;
                            }
                            other.GetComponent<BuildingPlane>().CostUpdate();
                        }

                    }





                }
                if (stackedMoneyCount == 0 && bankMoneyCount > 0)
                {
                    reduceBankMoneyTimer += Time.deltaTime;
                    if (reduceBankMoneyTimer > 0.05f)
                    {
                        reduceBankMoneyTimer = 0;
                        bankMoneyCount -= 1;
                        bankMoneyText.text = "$ " + bankMoneyCount;
                        totalMoneyCount -= 1;
                        other.GetComponent<BuildingPlane>().cost -= 1;
                        other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                        other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                        if (other.GetComponent<BuildingPlane>().cost == 0)
                        {
                            if (transform.tag == "PlayerRed")
                            {
                                other.GetComponent<BuildingPlane>().blue = false;
                                other.GetComponent<BuildingPlane>().red = true;
                                other.GetComponent<BuildingPlane>().yellow = false;
                                other.GetComponent<BuildingPlane>().green = false;
                            }
                            other.GetComponent<BuildingPlane>().CostUpdate();
                        }
                    }
                }


            }
        }
        
    }
}
