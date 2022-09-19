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
    


    private void Update()
    {
        if (moneyThrowBool)
        {
            throwMoneyTimer += Time.deltaTime;
        }
        
        rb.velocity = new Vector3(js.Horizontal * moveSpeed, rb.velocity.y, js.Vertical * moveSpeed);
        if (js.Horizontal != 0 || js.Vertical != 0)
        {
            if (rb.velocity != Vector3.zero)
            {
               // transform.rotation = Quaternion.Euler(0, rb.velocity.y, 0); ;
                  transform.rotation = Quaternion.LookRotation(rb.velocity);
                anim.SetInteger("movement", 1);
                moving = true;
            }
          
            

        }
        if (js.Horizontal == 0 && js.Vertical == 0 )
        {
            anim.SetInteger("movement", 0);
            moving = false;
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
        
            
            gm.stackedMoneyCount += 1;
            gm.totalMoneyCount += 1;
            gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
            gm.totalMoneyText.text = "Total : " + gm.totalMoneyCount;
            stackedMoneys.Add(other.gameObject);
            other.tag = "StackedMoney";
            other.transform.parent = stackTransform;
            other.transform.DOLocalJump(new Vector3(0, gm.stackedMoneyCount / 10, 0), 2, 1, 1);         
            other.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
      
        

    }
    
}
