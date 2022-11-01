using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingPlane : MonoBehaviour
{
    public GameMngr gm;
    public float startCost;
    public float cost;
    public float upgradeCost;
    public Text upgradeCostText;
    public Text costText;
    public Image amountImage;
    public int buildingLevel;
    public List<GameObject> buildingList;
    public Transform buildingGround;
    public Material[] materials;  // 1blue 2red 3green 4yellow
    public string color; 
    public List<GameObject> buildings;
   [HideInInspector] public float saleTimer;
    public GameObject textOneDollar;
    public GameObject canvasIncome;
   
    
    void Start()
    {
        startCost = Random.Range(25, 76);
        cost = startCost;
        costText.text = cost.ToString() + " $";
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMngr>();
        
      
    }
    private void Update()
    {
        if (color != null)
        {
            saleTimer += Time.deltaTime;
            if (color != "blue")
            {
                if (saleTimer > 1 / (startCost / 300))
                {
                    saleTimer = 0;
                    
                    if (color == "red")
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerRed");
                        player.GetComponent<Ai>().bankMoneyCount += 1;
                        player.GetComponent<Ai>().bankStack[player.GetComponent<Ai>().bankStackMode].transform.localScale += new Vector3(0, 0, player.GetComponent<Ai>().moneyScaleChange);

                        player.GetComponent<Ai>().bankStackMode += 1;
                        if (player.GetComponent<Ai>().bankStackMode == 9) { player.GetComponent<Ai>().bankStackMode = 0; }
                        player.GetComponent<Ai>().bankMoneyText.text = player.GetComponent<Ai>().bankMoneyText.text = "$ " + player.GetComponent<Ai>().bankMoneyCount;
                        player.GetComponent<Ai>().totalMoneyCount += 1;
                        var newText = Instantiate(textOneDollar, transform.position, Quaternion.Euler(90,0,0));
                        newText.transform.parent = canvasIncome.transform;
                        newText.transform.localPosition = new Vector3(0, buildingLevel + 2.5f, 0);
                       // newText.GetComponent<Text>().color = Color.red;
                       // Destroy(newText, 1f);
                    }
                    if (color == "green")
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerGreen");
                        player.GetComponent<Ai>().bankMoneyCount += 1;
                        player.GetComponent<Ai>().bankStack[player.GetComponent<Ai>().bankStackMode].transform.localScale += new Vector3(0, 0, player.GetComponent<Ai>().moneyScaleChange);

                        player.GetComponent<Ai>().bankStackMode += 1;
                        if (player.GetComponent<Ai>().bankStackMode == 9) { player.GetComponent<Ai>().bankStackMode = 0; }
                        player.GetComponent<Ai>().bankMoneyText.text = player.GetComponent<Ai>().bankMoneyText.text = "$ " + player.GetComponent<Ai>().bankMoneyCount;
                        player.GetComponent<Ai>().totalMoneyCount += 1;
                        var newText = Instantiate(textOneDollar, transform.position, Quaternion.Euler(90, 0, 0));
                        newText.transform.parent = canvasIncome.transform;
                        newText.transform.localPosition = new Vector3(0, buildingLevel + 2.5f, 0);
                       // newText.GetComponent<Text>().color = Color.green;
                      //  Destroy(newText, 1f);
                    }
                    if (color == "yellow")
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerYellow");
                        player.GetComponent<Ai>().bankMoneyCount += 1;
                        player.GetComponent<Ai>().bankStack[player.GetComponent<Ai>().bankStackMode].transform.localScale += new Vector3(0, 0, player.GetComponent<Ai>().moneyScaleChange);

                        player.GetComponent<Ai>().bankStackMode += 1;
                        if (player.GetComponent<Ai>().bankStackMode == 9) { player.GetComponent<Ai>().bankStackMode = 0; }
                        player.GetComponent<Ai>().bankMoneyText.text = player.GetComponent<Ai>().bankMoneyText.text = "$ " + player.GetComponent<Ai>().bankMoneyCount;
                        player.GetComponent<Ai>().totalMoneyCount += 1;
                        var newText = Instantiate(textOneDollar, transform.position, Quaternion.Euler(90, 0, 0));
                        newText.transform.parent = canvasIncome.transform;
                        newText.transform.localPosition = new Vector3(0, buildingLevel + 2.5f, 0);
                      //  newText.GetComponent<Text>().color = Color.yellow;
                       // Destroy(newText, 1f);
                    }
                    


                }
            }
            if (color == "blue")
            {
                if (saleTimer > 1 / ((gm.incomeLevel/3) *(startCost / 300)))
                {
                    saleTimer = 0;
                    var player = GameObject.FindGameObjectWithTag("PlayerBlue");
                    player.GetComponent<Character>().gm.bankMoneyCount += 1;
                    player.GetComponent<Character>().bankStack[player.GetComponent<Character>().bankStackMode].transform.localScale += new Vector3(0, 0, player.GetComponent<Character>().moneyScaleChange);

                    player.GetComponent<Character>().bankStackMode += 1;
                    if (player.GetComponent<Character>().bankStackMode == 9) { player.GetComponent<Character>().bankStackMode = 0; }
                    player.GetComponent<Character>().bankStack[player.GetComponent<Character>().bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BaseMap", new Vector2(1, player.GetComponent<Character>().bankStack[player.GetComponent<Character>().bankStackMode].transform.localScale.z / 300));
                    player.GetComponent<Character>().bankStack[player.GetComponent<Character>().bankStackMode].transform.GetComponent<MeshRenderer>().materials[0].SetTextureScale("_BumpMap", new Vector2(1, player.GetComponent<Character>().bankStack[player.GetComponent<Character>().bankStackMode].transform.localScale.z / 300));
                    player.GetComponent<Character>().gm.bankMoneyText.text = player.GetComponent<Character>().gm.bankMoneyText.text = "$ " + player.GetComponent<Character>().gm.bankMoneyCount;
                    player.GetComponent<Character>().gm.totalMoneyCount += 1;
                    player.GetComponent<Character>().gm.totalMoneyText.text = player.GetComponent<Character>().gm.totalMoneyCount.ToString();
                    var newText = Instantiate(textOneDollar, transform.position, Quaternion.identity);
                    newText.transform.parent = canvasIncome.transform;
                    newText.transform.localPosition = new Vector3(0, buildingLevel+2.5f, 0);
                 //   newText.GetComponent<Text>().color = Color.blue;
                  //  Destroy(newText, 1f);

                }
            }
           
           
        }
      

    }
    public void ChangeColor()
    {
        for (int i = 0; i < buildings.Count; i++)
        {

            if (color == "blue")
            {
                Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                mats[0] = materials[0];
                mats[1] = materials[1];

                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
            }
            if (color == "red")
            {

                Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                mats[0] = materials[2];
                mats[1] = materials[3];

                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
            }
            if (color == "green")
            {
                Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                mats[0] = materials[4];
                mats[1] = materials[5];

                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
            }
            if (color == "yellow")
            {
                Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                mats[0] = materials[6];
                mats[1] = materials[7];

                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
            }


        }

        //for (int i = 0; i < buildings.Count; i++)
        //{
        //    for (int j = 0; j < buildings[i].transform.childCount; j++)
        //    {
        //        if (color == "blue")
        //        {
        //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[0];
        //        }
        //        if (color == "red")
        //        {
        //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[1];
        //        }
        //        if (color == "green")
        //        {
        //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[2];
        //        }
        //        if (color == "yellow")
        //        {
        //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[3];
        //        }
        //    }
        //}

        cost = startCost;
        costText.text = cost.ToString() + " $";
        amountImage.fillAmount = 1 - (cost / startCost);
        if (color == "red")
        {
            if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
            {
                var playerYellow = GameObject.FindGameObjectWithTag("PlayerYellow");
                if (playerYellow.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerYellow.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerYellow.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerYellow.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }
            if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
            {
                var playerGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
                if (playerGreen.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerGreen.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerGreen.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerGreen.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }



        }
        if (color == "green")
        {
            if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
            {
                var playerRed = GameObject.FindGameObjectWithTag("PlayerRed");
                if (playerRed.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerRed.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerRed.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerRed.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }
            if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
            {
                var playerYellow = GameObject.FindGameObjectWithTag("PlayerYellow");
                if (playerYellow.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerYellow.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerYellow.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerYellow.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }


        }
        if (color == "yellow")
        {
            if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
            {
                var playerRed = GameObject.FindGameObjectWithTag("PlayerRed");
                if (playerRed.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerRed.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerRed.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerRed.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }
            if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
            {
                var playerGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
                if (playerGreen.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerGreen.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerGreen.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerGreen.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }

        }
        if (color == "blue")
        {
            if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
            {
                var playerRed = GameObject.FindGameObjectWithTag("PlayerRed");
                if (playerRed.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerRed.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerRed.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerRed.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }
            if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
            {
                var playerYellow = GameObject.FindGameObjectWithTag("PlayerYellow");
                if (playerYellow.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerYellow.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerYellow.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerYellow.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }
            if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
            {
                var playerGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
                if (playerGreen.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                {
                    playerGreen.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                }
                if (!playerGreen.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                {
                    playerGreen.GetComponent<Ai>().buyableAreas.Add(gameObject);
                }
            }


        }
    }
    public void CostUpdate()
    {
        
        
            if (cost <= 0)
            {
                var newBuilding = Instantiate(buildingList[0], buildingGround.position + new Vector3(0, 10, 0f), Quaternion.Euler(0, 0, 0));
                newBuilding.transform.parent = gameObject.transform;
            newBuilding.transform.localPosition = new Vector3(0, newBuilding.transform.localPosition.y, 4.6f);
            newBuilding.transform.localRotation = Quaternion.Euler(0, 0, 0);
          
            buildings.Add(newBuilding);
            for (int i = 0; i < buildings.Count; i++)
            {

                if (color == "blue")
                {
                    Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                    mats[0] = materials[1];
                    mats[1] = materials[0];

                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                }
                if (color == "red")
                {

                    Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                    mats[0] = materials[3];
                    mats[1] = materials[2];

                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                }
                if (color == "green")
                {
                    Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                    mats[0] = materials[5];
                    mats[1] = materials[4];

                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                }
                if (color == "yellow")
                {
                    Material[] mats = buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials;
                    mats[0] = materials[7];
                    mats[1] = materials[6];

                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                    buildings[i].transform.Find("cafe").GetComponent<MeshRenderer>().materials = mats;
                }


            }

            //for (int i = 0; i < buildings.Count; i++)
            //{
            //    for (int j = 0; j < buildings[i].transform.childCount; j++)
            //    {
            //        if (color == "blue")
            //        {
            //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[0];
            //        }
            //        if (color == "red")
            //        {
            //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[1];
            //        }
            //        if (color == "green")
            //        {
            //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[2];
            //        }
            //        if (color == "yellow")
            //        {
            //            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[3];
            //        }
            //    }
            //}

            buildingLevel++;
            //if (buildingLevel != 1)
            //{
            //    for (int i = 0; i < buildings.Count - 1; i++)
            //    {
            //        buildings[i].transform.GetChild(0).gameObject.SetActive(false);
            //    }
            //}

               cost = buildingLevel * 100;
            
                startCost = cost;
            if (color != "blue")
            {
                costText.text = startCost.ToString() + " $";
            }
            else
            {
                costText.text = cost.ToString() + " $";
            }
               
                amountImage.fillAmount = 1 - (cost / startCost);

            if (color == "red")
            {
                if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
                {
                    var playerYellow = GameObject.FindGameObjectWithTag("PlayerYellow");
                    if (playerYellow.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerYellow.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerYellow.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerYellow.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
                if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
                {
                    var playerGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
                    if (playerGreen.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerGreen.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerGreen.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerGreen.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
               


            }
            if (color == "green")
            {
                if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
                {
                    var playerRed = GameObject.FindGameObjectWithTag("PlayerRed");
                    if (playerRed.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerRed.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerRed.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerRed.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
                if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
                {
                    var playerYellow = GameObject.FindGameObjectWithTag("PlayerYellow");
                    if (playerYellow.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerYellow.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerYellow.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerYellow.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
               
              
            }
            if (color == "yellow")
            {
                if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
                {
                    var playerRed = GameObject.FindGameObjectWithTag("PlayerRed");
                    if (playerRed.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerRed.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerRed.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerRed.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
                if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
                {
                    var playerGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
                    if (playerGreen.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerGreen.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerGreen.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerGreen.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }

            }
            if (color == "blue")
            {
                if (GameObject.FindGameObjectWithTag("PlayerRed") != null)
                {
                    var playerRed = GameObject.FindGameObjectWithTag("PlayerRed");
                    if (playerRed.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerRed.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerRed.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerRed.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
                if (GameObject.FindGameObjectWithTag("PlayerYellow") != null)
                {
                    var playerYellow = GameObject.FindGameObjectWithTag("PlayerYellow");
                    if (playerYellow.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerYellow.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerYellow.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerYellow.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }
                if (GameObject.FindGameObjectWithTag("PlayerGreen") != null)
                {
                    var playerGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
                    if (playerGreen.GetComponent<Ai>().buildingAreas.Contains(gameObject))
                    {
                        playerGreen.GetComponent<Ai>().buildingAreas.Remove(gameObject);
                    }
                    if (!playerGreen.GetComponent<Ai>().buyableAreas.Contains(gameObject))
                    {
                        playerGreen.GetComponent<Ai>().buyableAreas.Add(gameObject);
                    }
                }


            }
        }
        
            
        
       


       

    }


}
