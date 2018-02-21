using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSplitter : MonoBehaviour {

    private GameObject plane;
    // Use this for initialization
	void Start ()
    {
        plane = GameObject.Find("Plane");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButton(0) == false)
        {
            
        }
	}
}
