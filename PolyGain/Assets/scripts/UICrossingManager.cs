using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrossingManager : MonoBehaviour {

    public bool crossDialogOpened { get; set; }
    public GameObject spawnOne;
    public GameObject spawnTwo;
    private GameObject plantOne;
    public GameObject plantTwo;
    public Canvas canvas_crossing;

    public GameObject Plant_Manager;

	// Use this for initialization
	void Start () {
        crossDialogOpened = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openCrossDialog()
    {
        crossDialogOpened = true;
        canvas_crossing.enabled = true;

        plantOne = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().createPlantToView(new Plant(spawnOne.transform.position, PlantManager.dnaOneToCross));
        plantTwo = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().createPlantToView(new Plant(spawnTwo.transform.position, PlantManager.dnaTwoToCross));

    }

    public void crossPlants()
    {
        int length = PlantManager.dnaOneToCross.Length;
        int locus = Random.Range(1, length);
        int reverse = Random.Range(1, 3);
        int[] dna = new int[length];

        for(int i = 0; i < length; i ++)
        {
            if(i < locus)
            {
                if (reverse == 1)
                {
                    dna[i] = PlantManager.dnaOneToCross[i];
                }
                else
                {
                    dna[i] = PlantManager.dnaTwoToCross[i];
                }
            }
            else
            {
                if (reverse == 1)
                {
                    dna[i] = PlantManager.dnaTwoToCross[i];
                }
                else
                {
                    dna[i] = PlantManager.dnaOneToCross[i];
                }
            }
        }
        Debug.Log("locus: " + locus);
        Debug.Log("new dna: " + dna[0].ToString() + dna[1] + dna[2] + dna[3] + dna[4] + dna[5] + dna[6]);
        GameObject.Find("DB_Manager").GetComponent<DatabaseManager>().addPlantToBag(dna);
    }

}
