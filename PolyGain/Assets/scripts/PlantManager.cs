using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject stalk_01;
    public GameObject cup_01;
    public GameObject cup_02;
    public Plant[] plants;

    //static public GameObject plantOneToCross;
    //static public GameObject plantTwoToCross;
    static public int[] dnaOneToCross;
    static public int[] dnaTwoToCross;

    static public bool CrossMode = false;
    
    public void addPlant(Vector3 position, int[] dna)
    {
        if (GameObject.Find("planeClickable_" + (position.x / 4.5) + "_" + (position.z / 4.5)).GetComponent<PlaneParameters>().Free)
        {
            Plant plant = new Plant(position, dna);
            createPlant(plant);

            GameObject.Find("DB_Manager").GetComponent<DatabaseManager>().addPlant(
                plant.Position.x, plant.Position.z, dna);
        }
    }

    public void createPlant(Plant plant)
    {
        GameObject newStalk = null;
        GameObject newCup = null;
        bool hasStalk = true;
        switch (plant.Dna[0])
        {
            case 0:
                hasStalk = false;
                break;
            case 1:
                newStalk = UnityEngine.Object.Instantiate(stalk_01);
                break;
            case 2:
                //newStalk = UnityEngine.Object.Instantiate(stalk_02);
                break;
        }
        
        switch (plant.Dna[1])
        {
            case 1:
                newCup = UnityEngine.Object.Instantiate(cup_01);
                break;
            case 2:
                newCup = UnityEngine.Object.Instantiate(cup_02);
                break;
        }

        if (hasStalk)
        {
            newStalk.AddComponent<PlantCreating>();
            newStalk.transform.position = new Vector3(plant.Position.x, 0, plant.Position.z);
            newCup.transform.position = newStalk.transform.GetChild(0).transform.position;
            newCup.transform.rotation = newStalk.transform.GetChild(0).transform.rotation;
            newCup.transform.parent = newStalk.transform;
            newStalk.tag = "plantRoot";
        }
        else
        {
            newCup.transform.position = plant.Position;
            newCup.tag = "plantRoot";
        }
        newCup.AddComponent<PlantCreating>();

        GameObject.Find("planeClickable_" + (plant.Position.x / 4.5) + "_" + (plant.Position.z / 4.5)).GetComponent<PlaneParameters>().Free = false;
    }

    public GameObject createPlantToView(Plant plant)
    {
        GameObject newStalk = null;
        GameObject newCup = null;
        bool hasStalk = true;
        switch (plant.Dna[0])
        {
            case 0:
                hasStalk = false;
                break;
            case 1:
                newStalk = UnityEngine.Object.Instantiate(stalk_01);
                break;
            case 2:
                //newStalk = UnityEngine.Object.Instantiate(stalk_02);
                break;
        }

        switch (plant.Dna[1])
        {
            case 1:
                newCup = UnityEngine.Object.Instantiate(cup_01);
                break;
            case 2:
                newCup = UnityEngine.Object.Instantiate(cup_02);
                break;
        }

        if (hasStalk)
        {
            //newStalk.AddComponent<PlantCreating>();
            newStalk.transform.position = new Vector3(plant.Position.x, 0, plant.Position.z);
            newCup.transform.position = newStalk.transform.GetChild(0).transform.position;
            newCup.transform.rotation = newStalk.transform.GetChild(0).transform.rotation;
            newCup.transform.parent = newStalk.transform;
            newStalk.AddComponent<RotatePlant>();
            return newStalk;
        }
        else
        {
            newCup.transform.position = plant.Position;
            //newCup.AddComponent<PlantCreating>();
            newCup.AddComponent<RotatePlant>();
            return newCup;
        }
        

    }

    public Plant getPlant(float x, float z)
    {
        return new Plant(new Vector3(x, 0, z), getDna(x, z));
    }

    public int[] getDna(float x, float z)
    {
        int[] dna = GameObject.Find("DB_Manager").GetComponent<DatabaseManager>().getDnaOfPlant(x, z);
        return dna;
    }

    public void removePlant(float x, float z)
    {
        GameObject.Find("DB_Manager").GetComponent<DatabaseManager>().removePlant(x, z);
    }

    public void clickPlantActions(GameObject plant)
    {
        Debug.Log("test1");
        GameObject.Find("UI_Plant_Manager").GetComponent<UIPlantManager>().openDialogs(plant);
    }

    public void loadPlants()
    {
        plants = GameObject.Find("DB_Manager").GetComponent<DatabaseManager>().getPlants();
        foreach (Plant p in plants)
        {
            createPlant(p);
        }
    }

}
