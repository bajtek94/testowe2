using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("Plant_Manager").GetComponent<PlantManager>().loadPlants();
	}


	// Update is called once per frame
	void Update () {
		
	}
    

	/*public GameObject findClickablePlane(float x, float z) {
		GameObject plane = null;
		GameObject[] planes = GameObject.FindGameObjectsWithTag ("planeClickable");
		foreach (GameObject p in planes) {
			if (p.transform.position.x == x && p.transform.position.z == z) {
				return p;
			}
		}
		return plane;

	}*/
    
}
