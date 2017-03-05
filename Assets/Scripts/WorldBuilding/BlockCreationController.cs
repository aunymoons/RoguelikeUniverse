using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreationController : MonoBehaviour {

    public bool isCreating;
    public Vector3 currentRotation;
    public Color currentColor;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(Color col)
    {
        currentColor = col;
    }

    public void SetRotation(Vector3 rot)
    {
        currentRotation = rot;
    }
}
