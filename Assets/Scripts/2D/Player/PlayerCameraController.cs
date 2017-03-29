using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    
    public Transform targetTransform;
    public float damping = 0.5f;
    Vector3 offset = new Vector3(0, 0, -10);
    public bool follow;
   
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(follow)
        CameraMovement();
	}

    void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, Time.deltaTime * damping);
    }

}
