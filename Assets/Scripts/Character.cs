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
    public GameObject maxText;
    public Transform moneyBox;
    public float buyCost;
    public GameObject[] bankStack;
    public int bankStackMode;
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    public bool pressed = false;
    public Camera OrtoCam;
    public float moneyScaleChange;
    private void Update()
    {
       // Swipe();
        if (Vector3.Distance(transform.position, moneyBox.position) > 2)
        {
            if (gm.upgradesPanel.activeSelf)
            {
                gm.upgradesPanel.SetActive(false);
             //   other.transform.GetChild(0).transform.DOLocalRotateQuaternion(Quaternion.Euler(-90, 0, 0), 0.2f);
            }
        }
        if (moneyThrowBool)
        {
            throwMoneyTimer += Time.deltaTime;
        }
        if (gm.bankMoneyCount == 0 )
        {
            
            for (int i = 0; i < bankStack.Length; i++)
            {
                bankStack[i].transform.localScale = new Vector3(bankStack[i].transform.localScale.x, bankStack[i].transform.localScale.y,0 );
                
            }
            bankStackMode = 0;
            bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z/ 300));
            bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z/ 300));
        }
        if (fall)
        {
            if (gm.stackedMoneyCount > 0)
            {
                for (int i = 0; i < gm.stackedMoneyCount; i++)
                {

                    stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(transform.position+new Vector3(Random.Range(-5f,5f),0.35f, Random.Range(-5f, 5f)), 2, 1, 0.5f);
                    stackedMoneys[stackTransform.childCount - 1].GetComponent<Money>().StartCoroutine(stackedMoneys[stackTransform.childCount - 1].GetComponent<Money>().Tag());
                    gm.stackedMoneyCount -= 1;
                    gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                    gm.totalMoneyCount -= 1;
                    gm.totalMoneyText.text = gm.totalMoneyCount.ToString();
                    stackedMoneys[stackTransform.childCount - 1].transform.rotation = Quaternion.Euler(-90, 0, 0);
                  //  stackedMoneys[stackTransform.childCount - 1].GetComponent<MeshRenderer>().enabled = true;
                    // stackedMoneys[stackTransform.childCount - 1].transform.tag = "Money";
                    stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                   // Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2);
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
            if (fallTimer > 1.75f)
            {
                fall = false;
                fallTimer = 0;
                GetComponent<CapsuleCollider>().isTrigger = false;
            }

           


        }
        else
        {

          //  Swipe();
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
                if(other.GetComponent<BuildingPlane>().color =="blue")
                {
                    if (!gm.upgraded)
                    {
                        gm.upgradeButton.SetActive(true);
                        gm.upgradeButtonCostText.text = other.GetComponent<BuildingPlane>().cost + " $";
                        gm.upgradeButtonLevelText.text = "LEVEL " + (other.GetComponent<BuildingPlane>().buildingLevel + 1).ToString();
                    }
                    else
                    {
                        gm.upgradeButton.SetActive(false);
                    }
                    
                   

                    if (gm.stackedMoneyCount == 0 && gm.bankMoneyCount > 0 && gm.upgraded)
                    {
                      
                       
                            GetComponent<CapsuleCollider>().isTrigger = true;
                        if (gm.bankMoneyCount > 9)
                        {
                            gm.bankMoneyCount -= 9;
                            gm.totalMoneyCount -= 9;
                            other.GetComponent<BuildingPlane>().cost -= 9;
                            for (int i = 0; i < bankStack.Length; i++)
                            {
                                bankStack[i].transform.localScale += new Vector3(0, 0, -moneyScaleChange);
                                bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z/ 300));
                                bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z/ 300));
                            }

                           

                        }
                        else
                        {
                            gm.bankMoneyCount -= 1;
                            gm.totalMoneyCount -= 1;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            bankStack[bankStackMode].transform.localScale += new Vector3(0, 0, -moneyScaleChange);
                            bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z/ 300));
                            bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z/ 300));

                            bankStackMode -= 1;
                            if (bankStackMode == -1) { bankStackMode = 8; }
                        }
                           
                            gm.bankMoneyText.text = "$ " + gm.bankMoneyCount;
                       

                        gm.totalMoneyText.text = gm.totalMoneyCount.ToString();
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.blue;
                            
                            other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost <= 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (other.GetComponent<BuildingPlane>().color != "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "blue";
                                        other.GetComponent<BuildingPlane>().ChangeColor();
                                        gm.upgraded = false;

                                    }
                                    if (other.GetComponent<BuildingPlane>().color == "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                        gm.upgraded = false;


                                    }
                                }
                                else
                                {
                                    other.GetComponent<BuildingPlane>().color = "blue";
                                    other.GetComponent<BuildingPlane>().CostUpdate();
                                    gm.upgraded = false;
                                }





                            }
                        }
                    
                    if (gm.stackedMoneyCount > 0 && gm.upgraded)
                    {
                        moneyThrowBool = true;

                        if (throwMoneyTimer > 0.05f)
                        {
                            GetComponent<CapsuleCollider>().isTrigger = true;
                            throwMoneyTimer = 0;
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position + new Vector3(0, -3, 0), 5, 1, 1);
                            //  stackTransform.GetChild(stackTransform.childCount - 1).transform.DOMove(other.transform.position, 0.1f);
                            gm.stackedMoneyCount -= 1;
                            gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                            gm.totalMoneyCount -= 1;
                            gm.totalMoneyText.text = gm.totalMoneyCount.ToString();
                            stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                            Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 0.5f);
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.blue;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost <= 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (other.GetComponent<BuildingPlane>().color != "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "blue";
                                        other.GetComponent<BuildingPlane>().ChangeColor();
                                        gm.upgraded = false;
                                    }
                                    if (other.GetComponent<BuildingPlane>().color == "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                        gm.upgraded = false;

                                    }
                                }
                                else
                                {
                                    other.GetComponent<BuildingPlane>().color = "blue";
                                    other.GetComponent<BuildingPlane>().CostUpdate();
                                    gm.upgraded = false;
                                }





                            }
                        }





                    }
                }
                else if (other.GetComponent<BuildingPlane>().color == "")
                {
                    if (gm.stackedMoneyCount == 0 && gm.bankMoneyCount > 0)
                    {
                      
                            GetComponent<CapsuleCollider>().isTrigger = true;
                            reduceBankMoneyTimer = 0;
                            gm.bankMoneyCount -= 1;
                            gm.bankMoneyText.text = "$ " + gm.bankMoneyCount;
                        bankStackMode -= 1;
                        if (bankStackMode == -1)
                        {
                            bankStackMode = 8;
                        }
                        bankStack[bankStackMode].transform.localScale += new Vector3(0, 0, -moneyScaleChange);
                        bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z / 300));
                        bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z / 300));


                        gm.totalMoneyCount -= 1;
                            gm.totalMoneyText.text = gm.totalMoneyCount.ToString();
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.blue;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost <= 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (other.GetComponent<BuildingPlane>().color != "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "blue";
                                        other.GetComponent<BuildingPlane>().ChangeColor();
                                        gm.upgraded = false;

                                    }
                                    if (other.GetComponent<BuildingPlane>().color == "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                        gm.upgraded = false;


                                    }
                                }
                                else
                                {
                                    other.GetComponent<BuildingPlane>().color = "blue";
                                    other.GetComponent<BuildingPlane>().CostUpdate();
                                    gm.upgraded = false;
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
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position + new Vector3(0, -3, 0), 5, 1, 1);
                            //  stackTransform.GetChild(stackTransform.childCount - 1).transform.DOMove(other.transform.position, 0.1f);
                            gm.stackedMoneyCount -= 1;
                            gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                            gm.totalMoneyCount -= 1;
                            gm.totalMoneyText.text = gm.totalMoneyCount.ToString();
                            stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                            Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 0.5f);
                            stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                            other.GetComponent<BuildingPlane>().amountImage.color = Color.blue;
                            other.GetComponent<BuildingPlane>().cost -= 1;
                            other.GetComponent<BuildingPlane>().costText.text = other.GetComponent<BuildingPlane>().cost.ToString() + " $";
                            other.GetComponent<BuildingPlane>().amountImage.fillAmount = 1 - (other.GetComponent<BuildingPlane>().cost / other.GetComponent<BuildingPlane>().startCost);
                            if (other.GetComponent<BuildingPlane>().cost <= 0)
                            {
                                if (other.GetComponent<BuildingPlane>().buildingLevel != 0)
                                {
                                    if (other.GetComponent<BuildingPlane>().color != "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().color = "blue";
                                        other.GetComponent<BuildingPlane>().ChangeColor();
                                        gm.upgraded = false;
                                    }
                                    if (other.GetComponent<BuildingPlane>().color == "blue")
                                    {
                                        other.GetComponent<BuildingPlane>().CostUpdate();
                                        gm.upgraded = false;

                                    }
                                }
                                else
                                {
                                    other.GetComponent<BuildingPlane>().color = "blue";
                                    other.GetComponent<BuildingPlane>().CostUpdate();
                                    gm.upgraded = false;
                                }





                            }
                        }





                    }
                }
                else
                {
                    gm.upgraded = false;
                    gm.upgradeButton.SetActive(false);
                    gm.buyButton.SetActive(true);
                    buyCost = other.GetComponent<BuildingPlane>().startCost;
                    gm.buyButtonCostText.text = other.GetComponent<BuildingPlane>().startCost + " $";
                    gm.buyButtonLevelText.text = "LEVEL "+ other.GetComponent<BuildingPlane>().buildingLevel.ToString();
                    if (gm.buildingBought)
                    {
                        
                        other.GetComponent<BuildingPlane>().color = "blue";
                        other.GetComponent<BuildingPlane>().ChangeColor();
                        gm.buildingBought = false;
                    }
                }




            }
            if (other.tag == "MoneyBoxBlue")
            {
                gm.upgradesPanel.SetActive(true);
                if (gm.stackedMoneyCount > 0)
                {

                    moneyThrowBool = true;

                    if (throwMoneyTimer > 0.05f)
                    {
                        
                        GetComponent<CapsuleCollider>().isTrigger = true;
                        throwMoneyTimer = 0;
                     //   other.transform.GetChild(0).transform.DOLocalRotateQuaternion(Quaternion.Euler(-90, -90, 0), 0.2f);
                       stackTransform.GetChild(stackTransform.childCount - 1).transform.DOJump(other.transform.position+new Vector3(0,-0.5f,0), 3, 1, 1);
                        
                        gm.stackedMoneyCount -= 1;
                        gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                        gm.bankMoneyCount += 1;
                        gm.bankMoneyText.text = "$ " + gm.bankMoneyCount;
                        stackedMoneys.Remove(stackTransform.GetChild(stackTransform.childCount - 1).gameObject);
                        Destroy(stackTransform.GetChild(stackTransform.childCount - 1).gameObject, 2f);
                        stackTransform.GetChild(stackTransform.childCount - 1).transform.parent = null;
                        bankStack[bankStackMode].transform.localScale += new Vector3(0, 0, moneyScaleChange);
                        bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z / 300));
                        bankStack[bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, bankStack[bankStackMode].transform.localScale.z / 300));
                        //moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BaseMap", new Vector2(1, moneyStack.GetComponent<MeshRenderer>().materials[1].GetTextureScale("_BaseMap").y + 1));
                        //moneyStack.GetComponent<MeshRenderer>().materials[1].SetTextureScale("_BumpMap", new Vector2(1, moneyStack.GetComponent<MeshRenderer>().materials[1].GetTextureScale("_BumpMap").y + 1));
                        bankStackMode += 1;
                        if (bankStackMode == 9) { bankStackMode = 0; }
                    }

                }




            }
           
        }

        if (other.tag == "Money" && gm.stackedMoneyCount >= gm.maxStackedCount && maxText.activeSelf == false)
        {
            maxText.SetActive(true);
            LeanTween.delayedCall(gameObject, 0.5f, () => { maxText.SetActive(false); });

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Money")
        {
            if (gm.stackedMoneyCount < gm.maxStackedCount)
            {
                gm.stackedMoneyCount += 1;
                gm.totalMoneyCount += 1;
                gm.stackedMoneyText.text = "Stacked : " + gm.stackedMoneyCount;
                gm.totalMoneyText.text =  gm.totalMoneyCount.ToString();
                stackedMoneys.Add(other.gameObject);
                other.tag = "StackedMoney";
                other.transform.parent = stackTransform;
                //   other.GetComponent<MeshRenderer>().enabled = false;
                other.transform.DOLocalMove(new Vector3(0, gm.stackedMoneyCount, 0), 0.2f);
                //other.transform.GetChild(0).gameObject.SetActive(true);
                //other.transform.GetChild(1).gameObject.SetActive(true);
                //other.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
                //other.transform.GetChild(1).GetComponent<TrailRenderer>().enabled = true;
                //other.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color(0, 0, 1, 1);
                //other.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(0, 0, 1, 0);
                //other.transform.GetChild(1).GetComponent<TrailRenderer>().startColor = new Color(0, 0, 1, 1);
                //other.transform.GetChild(1).GetComponent<TrailRenderer>().endColor = new Color(0, 0, 1, 0); 
                //    Destroy(other.transform.GetChild(1).gameObject, 0.5f);
                //   Destroy(other.transform.GetChild(0).gameObject, 0.5f);
                // other.transform.DOLocalJump(new Vector3(0, gm.stackedMoneyCount, 0), 0, 1, 0.2f);
                other.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }

        }
    }
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "PlayerRed" || collision.gameObject.tag == "PlayerGreen" || collision.gameObject.tag == "PlayerYellow")
        {
            if (gm.stackedMoneyCount >= collision.gameObject.GetComponent<Ai>().stackedMoneyCount && gm.stackedMoneyCount !=0)
            {
                collision.gameObject.GetComponent<Ai>().fall = true;
                collision.gameObject.GetComponent<Ai>().anim.SetBool("fall", true);
            }
        }

               
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MoneyBoxBlue")
        {
            gm.upgradesPanel.SetActive(false);
          //  other.transform.GetChild(0).transform.DOLocalRotateQuaternion(Quaternion.Euler(-90, 0, 0), 0.2f);
        }
        if (other.tag == "BuildingPlane")
        {
            gm.buyButton.SetActive(false);
            buyCost = 0;
            gm.upgradeButton.SetActive(false);
            gm.upgraded = false;
        }
    }

    public void Swipe()
    {

        //if (this.transform.position.y < -3)
        //{
        //    this.transform.position = this.transform.position + new Vector3(0, 6, 0);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            pressed = true;
            //Vcam4.Priority = 12;

            //if (!isInPool) { Anim.SetBool("walk", true); m_AudioSource1.Play(); }
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = OrtoCam.ScreenToWorldPoint(Input.mousePosition);
            pressed = false;
            //Vcam4.Priority = 8;

            //if (!isInPool) { Anim.SetBool("walk", false); m_AudioSource1.Pause(); }
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
            transform.position = transform.position + (transform.forward * moveSpeed * Time.deltaTime);
            Vector3 pos1 = transform.position;
            pos1.y = Mathf.Clamp(pos1.y, -1, 2);
            transform.position = pos1;
        }


    }
}
