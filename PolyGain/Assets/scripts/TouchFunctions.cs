using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		fingerBeganPosition = Vector2.zero;
	}

	public GameObject plant01;
	public GameObject plant02;
	private Vector2 fingerBeganPosition;
    private float speed = 0.02F;

    // Update is called once per frame
    void Update()
	{
        if (!GameObject.Find("UI_Manager").GetComponent<UIBagManager>().bagOpened)
        {
            touchAction();
            if (!GameObject.Find("UI_Plant_Manager").GetComponent<UIPlantManager>().dialogsOpened) {
                moveCamera();
            }
        }
	}

	private void touchAction() {

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			switch(touch.phase) {
			case TouchPhase.Began:
				fingerBeganPosition = Input.GetTouch (0).position;
				break;
			case TouchPhase.Ended:
                    if (GameObject.Find("UI_Plant_Manager").GetComponent<UIPlantManager>().dialogsOpened)
                    {
                        GameObject.Find("UI_Plant_Manager").GetComponent<UIPlantManager>().closeDialogs();
                    }
                    else
                    {
                        float dist = Vector2.Distance(fingerBeganPosition, Input.GetTouch(0).position);
                        if (dist < 10)
                        {
                            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                            RaycastHit raycastHit;
                            if (Physics.Raycast(raycast, out raycastHit))
                            {
                                GameObject touchedObject = raycastHit.collider.gameObject;
                                if ((PlantManager.CrossMode == true) && (touchedObject.CompareTag("plant") || touchedObject.CompareTag("plantRoot")))
                                {
                                    PlantManager.dnaTwoToCross = null;
                                    int[] dnaOfTouched;
                                    
                                    if(touchedObject.transform.parent != null)
                                    {
                                        dnaOfTouched = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().getDna(touchedObject.transform.parent.transform.position.x, touchedObject.transform.parent.transform.position.z);
                                    }
                                    else
                                    {
                                        dnaOfTouched = GameObject.Find("Plant_Manager").GetComponent<PlantManager>().getDna(touchedObject.transform.position.x, touchedObject.transform.position.z);
                                    }
                                    
                                    if(!Enumerable.SequenceEqual(dnaOfTouched, PlantManager.dnaOneToCross))
                                    {
                                        Debug.Log(PlantManager.dnaOneToCross[0].ToString() + PlantManager.dnaOneToCross[1] + PlantManager.dnaOneToCross[2] + PlantManager.dnaOneToCross[3] + PlantManager.dnaOneToCross[4] + PlantManager.dnaOneToCross[5] + PlantManager.dnaOneToCross[6]);
                                        Debug.Log(dnaOfTouched[0].ToString() + dnaOfTouched[1] + dnaOfTouched[2] + dnaOfTouched[3] + dnaOfTouched[4] + dnaOfTouched[5] + dnaOfTouched[6]);                                        
                                        PlantManager.dnaTwoToCross = dnaOfTouched;
                                    }
                                    if (PlantManager.dnaTwoToCross != null)
                                    {
                                        //// ODPALENIE NOWEGO UI Z KRZYŻOWANIEM W TYM MIEJSCU
                                        GameObject.Find("UI_Crossing_Manager").GetComponent<UICrossingManager>().openCrossDialog();
                                    }
                                    
                                }
                                else if ((PlantManager.CrossMode == false) && touchedObject.CompareTag("planeClickable"))
                                {
                                    if (touchedObject.GetComponent<PlaneParameters>().Free)
                                    {
                                        GameObject.Find("UI_Manager").GetComponent<UIBagManager>().openBagWindow(touchedObject.transform.position);
                                    }
                                }
                                else if ((PlantManager.CrossMode == false) && (touchedObject.CompareTag("plant") || touchedObject.CompareTag("plantRoot")))
                                {
                                    Debug.Log("test0");
                                    GameObject.Find("Plant_Manager").GetComponent<PlantManager>().clickPlantActions(touchedObject);
                                }
                            }
                        }
                    }
				break;
			}

		}

	}

    public void moveCamera()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 moves = Input.GetTouch(0).deltaPosition * speed;
            transform.Translate(-moves);
        }
    }

}
