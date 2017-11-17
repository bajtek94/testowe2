using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreating : MonoBehaviour {

    private Quaternion locRotation;
    private float x;
    private float angleDist = 4;

	// Use this for initialization
	void Start () {
        x = 0;
        locRotation = transform.rotation;
        transform.Rotate(0, 0, 350);
		transform.localScale = Vector3.zero;
        if (CompareTag("plantRoot"))
        {
            if (GameObject.Find("planeClickable_" + (transform.position.x / 4.5) + "_" + (transform.position.z / 4.5)) != null)
            {
                GameObject.Find("planeClickable_" + (transform.position.x / 4.5) + "_" + (transform.position.z / 4.5)).GetComponent<PlaneParameters>().Free = false;
            }
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {        
        if (transform.localScale.x < 1)
        {
            transform.localScale += new Vector3(0.02F, 0.02F, 0);
        }
        if (transform.localScale.z < 1)
        {
            transform.localScale += new Vector3(0, 0, 0.015F);
        }
        if (transform.rotation.z > locRotation.z && CompareTag("plantRoot"))
        {
            transform.Rotate(new Vector3(0, 0, -angleDist*Mathf.Sin(x)));
            x += Mathf.PI/(angleDist*35);
        }
        
    }
}
