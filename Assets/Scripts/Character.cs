using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Character : MonoBehaviour
{
    public Rigidbody rb;
    public DynamicJoystick js;
    public Animator anim;
    public float moveSpeed;
   
    public List<GameObject> stackedMoneys;
    public Transform stackTransform;
    float reduceBankMoneyTimer;
    bool moneyThrowBool;
    public float throwMoneyTimer;
    public GameMngr gm;
    public bool moving;
    public bool fall;
    float fallTimer;

    private void Update()
    {
        if (moneyThrowBool)
        {
            throwMoneyTimer += Time.deltaTime;
        }
        if (fall)
        {
            if (gm.stackedMoneyCount > 0)
            {
                for (int i = 0; i < gm.stackedMoneyCount; i++)
                {
                    stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(transform.position+new Vector3(Random.Range(-5f,5f),0.2f, Random.Range(-5f, 5f)), 2, 1, 1);
                    gm.stackedMoneyCount -= 1;
                    gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                    gm.totalMoneyCount -= 1;
                    gm.totalMoneyText.text = "$ " + gm.totalMoneyCount;
                   // stackedMoneys[stackTransform.childCount - 1].transform.tag = "Money";
                    stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                    Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2);
                    stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                }
            }
            GetComponent<CapsuleCollider>().isTrigger = true;
            rb.velocity = Vector3.zero;
            fallTimer += Time.deltaTime;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
            {
                anim.SetInteger("movement", 0);
                
            }
            if (fallTimer > 3.5f)
            {
                fall = false;
                fallTimer = 0;
                GetComponent<CapsuleCollider>().isTrigger = false;
            }

           


        }
        else
        {
            rb.velocity = new Vector3(js.Horizontal * moveSpeed, rb.velocity.y, js.Vertical * moveSpeed);
            if (js.Horizontal != 0 || js.Vertical != 0)
            {
                if (rb.velocity != Vector3.zero)
                {
                    // transform.rotation = Quaternion.Euler(0, rb.velocity.y, 0); ;
                    transform.rotation = Quaternion.LookRotation(rb.velocity);
                    anim.SetInteger("movement", 1);
                    moving = true;                
                    GetComponent<CapsuleCollider>().isTrigger = false;
                    
                }



            }
            if (js.Horizontal == 0 && js.Vertical == 0)
            {
                anim.SetInteger("movement", 0);
                moving = false;
            }
        }
       
       
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!moving)
        {
            if (other.tag == "BuildingPlane")
            {

                if (gm.stackedMoneyCount == 0 && gm.bankMoneyCount > 0)
                {
                    reduceBankMoneyTimer += Time.deltaTime;
                    if (reduceBankMoneyTimer > 0.05f)
                    {
                        GetComponent<CapsuleCollider>().isTrigger = true;
                        reduceBankMoneyTimer = 0;
                        gm.bankMoneyCount -= 1;
                        gm.bankMoneyText.text = "$ " + gm.bankMoneyCount;
                        gm.totalMoneyCount -= 1;
                        gm.totalMoneyText.text = "Total : " + gm.totalMoneyCount;
                        other.GetComponent<BuildingPlane>().cost -= 1;
                        other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                        other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                        if (other.GetComponent<BuildingPlane>().cost == 0)
                        {
                            if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                            {
                                if (other.GetComponent<BuildingPlane>().color != "blue")
                                {
                                    other.GetComponent<BuildingPlane>().color = "blue";
                                    other.GetComponent<BuildingPlane>().ChangeColor();

                                }
                                if (other.GetComponent<BuildingPlane>().color == "blue")
                                {
                                    other.GetComponent<BuildingPlane>().CostUpdate();
                                   

                                }
                            }
                            else
                            {
                                other.GetComponent<BuildingPlane>().color = "blue";
                                other.GetComponent<BuildingPlane>().CostUpdate();
                            }





                        }
                    }
                }
                if (gm.stackedMoneyCount > 0)
                {
                    moneyThrowBool = true;

                    if (throwMoneyTimer > 0.05f)
                    {
                        GetComponent<CapsuleCollider>().isTrigger = true;
                        throwMoneyTimer = 0;
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position, 2, 1, 1);                       
                        gm.stackedMoneyCount -= 1;
                        gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                        gm.totalMoneyCount -= 1;
                        gm.totalMoneyText.text = "Total : " + gm.totalMoneyCount;
                        stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                        Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject,2);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                        other.GetComponent<BuildingPlane>().cost -= 1;
                        other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                        other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                        if (other.GetComponent<BuildingPlane>().cost == 0)
                        {
                            if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                            {
                                if (other.GetComponent<BuildingPlane>().color != "blue")
                                {
                                    other.GetComponent<BuildingPlane>().color = "blue";
                                    other.GetComponent<BuildingPlane>().ChangeColor();

                                }
                                if (other.GetComponent<BuildingPlane>().color == "blue")
                                {
                                    other.GetComponent<BuildingPlane>().CostUpdate();
                                    

                                }
                            }
                            else
                            {
                                other.GetComponent<BuildingPlane>().color = "blue";
                                other.GetComponent<BuildingPlane>().CostUpdate();
                            }


                            

                           
                        }
                    }





                }




            }
            if (other.tag == "MoneyBox")
            {

                if (gm.stackedMoneyCount > 0)
                {

                    moneyThrowBool = true;

                    if (throwMoneyTimer > 0.05f)
                    {
                        GetComponent<CapsuleCollider>().isTrigger = true;
                        throwMoneyTimer = 0;
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position, 2, 1, 1);
                        gm.stackedMoneyCount -= 1;
                        gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                        gm.bankMoneyCount += 1;
                        gm.bankMoneyText.text = "$ " + gm.bankMoneyCount;
                        stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                        Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                    }

                }




            }
        }
           
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Money")
        {
            if (gm.stackedMoneyCount <= gm.maxStackedCount)
            {
                gm.stackedMoneyCount += 1;
                gm.totalMoneyCount += 1;
                gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                gm.totalMoneyText.text = "Total : " + gm.totalMoneyCount;
                stackedMoneys.Add(other.gameObject);
                other.tag = "StackedMoney";
                other.transform.parent = stackTransform;

                other.transform.DOLocalJump(new Vector3(0, gm.stackedMoneyCount, 0), 0, 1, 0.2f);
                other.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
           
        }
    }
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "PlayerRed" || collision.gameObject.tag == "PlayerGreen" || collision.gameObject.tag == "PlayerYellow")
        {
            if (gm.stackedMoneyCount > collision.gameObject.GetComponent<Ai>().stackedMoneyCount)
            {
                collision.gameObject.GetComponent<Ai>().fall = true;
                collision.gameObject.GetComponent<Ai>().anim.SetBool("fall", true);
            }
        }

               
       
    }
}
