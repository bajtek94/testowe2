using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInBag : MonoBehaviour {

    public int[] dna;
    public int count;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public PlantInBag(int[] dna, int count)
    {
        this.dna = dna;
        this.count = count;
    }



}
