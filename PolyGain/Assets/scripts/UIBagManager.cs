using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagManager : MonoBehaviour {

    public bool bagOpened{ get; set; }
    private Vector3 spawnPosition;
    public Canvas canvas_bag;
    private PlantInBag[] bag;
    private int bagIndex;
    private int bagSize;
    GameObject plantInDialog;
    private Vector3 positionToPlant;

    // Use this for initialization
    void Start () {
        bagOpened = false;
        bagIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    


    public void openBagWindow(Vector3 pos)
    {
        positionToPlant = new Vector3(pos.x, 0, pos.z);


        //// Załadowanie z DB plecaka
        //// Na razie tylko przykładowe na sztywno
        bagIndex = 0;
        //Plant a = new Plant(spawnPosition, new int[] { 1, 1, 0, 0, 0, 0, 0 });
        //Plant b = new Plant(spawnPosition, new int[] { 0, 2, 0, 0, 0, 0, 0 });
        bag = GameObject.Find("DB_Manager").GetComponent<DatabaseManager>().getBagPlants();
        bagSize = bag.Length;
        Debug.Log(bagSize);

        canvas_bag.enabled = true;
        bagOpened = true;

        spawnPosition = GameObject.Find("PlantSpawner").GetComponent<Transform>().position;
        plantInDialog = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().createPlantToView(new Plant(spawnPosition, bag[bagIndex].dna));


        setActives();


    }
    public void closeBagWindow()
    {
        canvas_bag.enabled = false;
        bagOpened = false;
        Destroy(plantInDialog);
    }

    public void bagNextPlant()
    {
        if(bagIndex < bagSize-1)
        {
            bagIndex++;
            Destroy(plantInDialog);
            plantInDialog = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().createPlantToView(new Plant(spawnPosition, bag[bagIndex].dna));
        }
        setActives();
    }

    public void bagPreviousPlant()
    {
        if (bagIndex>0)
        {
            bagIndex--;
            Destroy(plantInDialog);
            plantInDialog = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().createPlantToView(new Plant(spawnPosition, bag[bagIndex].dna));
        }
        setActives();
    }

    public void confirmPlantClick()
    {
        GameObject.Find("Plant_Manager").GetComponent<PlantManager>().addPlant(positionToPlant, bag[bagIndex].dna);
        Destroy(plantInDialog);
        closeBagWindow();
    }

    public void setActives()
    {
        if(bagIndex== 0)
        {
            GameObject.Find("btn_bag_left").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("btn_bag_left").GetComponent<Button>().interactable = true;
        }
        if (bagIndex == bagSize-1)
        {
            GameObject.Find("btn_bag_right").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("btn_bag_right").GetComponent<Button>().interactable = true;
        }
    }

    


}
