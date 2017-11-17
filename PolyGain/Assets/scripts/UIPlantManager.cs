using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlantManager : MonoBehaviour {

    public bool dialogsOpened { get; set; }
    private Vector3 positionOfPlantInWorld;
    private Vector3 positionOfPlantOnScreen;
    public Canvas canvas_plantOptions;
    public Button btn_sell;
    public Button btn_cross;
    public Image img_circle;
    
    //private string[] options;
    public Camera cam;
    static GameObject plant;
    


    // Use this for initialization
    void Start () {
        dialogsOpened = false;
        //options = new string[] { "krzyżuj", "sprzedaj" };
        canvas_plantOptions.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openDialogs(GameObject p)
    {
        PlantManager.dnaOneToCross = null;
        PlantManager.dnaTwoToCross = null;
        PlantManager.CrossMode = false;

       canvas_plantOptions.enabled = true;
        plant = p;
        positionOfPlantInWorld = p.transform.position;
        positionOfPlantOnScreen = cam.WorldToScreenPoint(p.transform.position);
        setPositionOfButtons();
        dialogsOpened = true;
    }

    public void closeDialogs()
    {
        canvas_plantOptions.enabled = false;
        dialogsOpened = false;
    }

    public void setPositionOfButtons()
    {
        btn_sell.transform.position = new Vector3(positionOfPlantOnScreen.x, positionOfPlantOnScreen.y + 50);
        btn_cross.transform.position = new Vector3(positionOfPlantOnScreen.x, positionOfPlantOnScreen.y - 50);
        img_circle.transform.position = new Vector3(positionOfPlantOnScreen.x, positionOfPlantOnScreen.y);
    }

    public void sellPlant()
    {
        Debug.Log("Click sell");        
        
        if (plant.transform.parent != null)
        {
            GameObject.Find("Plant_Manager").GetComponent<PlantManager>().removePlant(plant.transform.parent.position.x, plant.transform.parent.position.z);
            GameObject.Find("planeClickable_" + (plant.transform.parent.position.x / 4.5) + "_" + (plant.transform.parent.position.z / 4.5)).GetComponent<PlaneParameters>().Free = true;
            Destroy(plant.transform.parent.gameObject);
        }
        else
        {
            GameObject.Find("Plant_Manager").GetComponent<PlantManager>().removePlant(positionOfPlantInWorld.x, positionOfPlantInWorld.z);
            GameObject.Find("planeClickable_" + (plant.transform.position.x / 4.5) + "_" + (plant.transform.position.z / 4.5)).GetComponent<PlaneParameters>().Free = true;
            Destroy(plant);
        }
    }

    public void crossPlant()
    {
        Debug.Log("Click cross");

        if (plant.transform.parent != null)
        {
            PlantManager.dnaOneToCross = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().getDna(plant.transform.parent.transform.position.x, plant.transform.parent.transform.position.z);
        }
        else
        {
            PlantManager.dnaOneToCross = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().getDna(plant.transform.position.x, plant.transform.position.z);
        }
        PlantManager.CrossMode = true;
        
    }


}
