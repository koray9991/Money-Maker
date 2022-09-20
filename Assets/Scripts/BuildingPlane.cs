using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingPlane : MonoBehaviour
{
    public GameMngr gm;
    public float startCost;
    public float cost;
    public Text costText;
    public Image amountImage;
    public int buildingLevel;
    public List<GameObject> buildingList;
    public Transform buildingGround;
    public Material[] materials;  // 1blue 2red 3green 4yellow
    public string color; 
    public List<GameObject> buildings;
   [HideInInspector] public float saleTimer;
    void Start()
    {
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
                if (saleTimer > 1 / (startCost / 200))
                {
                    saleTimer = 0;
                    if (color == "red")
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerRed");
                        player.GetComponent<Ai>().bankMoneyCount += 1;
                        player.GetComponent<Ai>().bankMoneyText.text = player.GetComponent<Ai>().bankMoneyText.text = "$ " + player.GetComponent<Ai>().bankMoneyCount;
                        player.GetComponent<Ai>().totalMoneyCount += 1;
                    }
                    if (color == "green")
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerGreen");
                        player.GetComponent<Ai>().bankMoneyCount += 1;
                        player.GetComponent<Ai>().bankMoneyText.text = player.GetComponent<Ai>().bankMoneyText.text = "$ " + player.GetComponent<Ai>().bankMoneyCount;
                        player.GetComponent<Ai>().totalMoneyCount += 1;
                    }
                    if (color == "yellow")
                    {
                        var player = GameObject.FindGameObjectWithTag("PlayerYellow");
                        player.GetComponent<Ai>().bankMoneyCount += 1;
                        player.GetComponent<Ai>().bankMoneyText.text = player.GetComponent<Ai>().bankMoneyText.text = "$ " + player.GetComponent<Ai>().bankMoneyCount;
                        player.GetComponent<Ai>().totalMoneyCount += 1;
                    }
                    


                }
            }
            if (color == "blue")
            {
                if (saleTimer > 1 / ((gm.incomeLevel/3) *(startCost / 200)))
                {
                    saleTimer = 0;
                    var player = GameObject.FindGameObjectWithTag("PlayerBlue");
                    player.GetComponent<Character>().gm.bankMoneyCount += 1;
                    player.GetComponent<Character>().gm.bankMoneyText.text = player.GetComponent<Character>().gm.bankMoneyText.text = "$ " + player.GetComponent<Character>().gm.bankMoneyCount;
                    player.GetComponent<Character>().gm.totalMoneyCount += 1;
                }
            }
           
           
        }
      

    }
    public void ChangeColor()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            for (int j = 0; j < buildings[i].transform.childCount; j++)
            {
                if (color == "blue")
                {
                    buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[0];
                }
                if (color == "red")
                {
                    buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[1];
                }
                if (color == "green")
                {
                    buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[2];
                }
                if (color == "yellow")
                {
                    buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[3];
                }
            }
        }
        
        cost = startCost;
        costText.text = cost.ToString() + " $";
        amountImage.fillAmount = 1 - (cost / startCost);
    }
    public void CostUpdate()
    {
        
        
            if (cost == 0)
            {
                var newBuilding = Instantiate(buildingList[0], buildingGround.position + new Vector3(0, 10, 0f), Quaternion.Euler(0, 90, 0));
                newBuilding.transform.parent = gameObject.transform;
                buildings.Add(newBuilding);
                for (int i = 0; i < buildings.Count; i++)
                {
                    for (int j = 0; j < buildings[i].transform.childCount; j++)
                    {
                        if (color == "blue")
                        {
                            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[0];
                        }
                        if (color == "red")
                        {
                            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[1];
                        }
                        if (color == "green")
                        {
                            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[2];
                        }
                        if (color == "yellow")
                        {
                            buildings[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[3];
                        }
                    }
                }

                buildingLevel++;
                cost = buildingLevel * startCost + startCost;
                startCost = cost;
                costText.text = cost.ToString() + " $";
                amountImage.fillAmount = 1 - (cost / startCost);

            }
        
            
        
       


       

    }


}
