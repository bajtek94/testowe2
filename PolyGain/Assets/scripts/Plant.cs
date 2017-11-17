using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant{
    
    public Vector3 Position { get; set; }
    public int[] Dna { get; set; }
    public GameObject stalk;
    public GameObject cup;

    public Plant(Vector3 position, int[] dna)
    {
        Position = position;
        Dna = dna;
    }



}
