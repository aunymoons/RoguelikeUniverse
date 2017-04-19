using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    
    //References
    Transform targetTransform;
    public float damping = 0.8f;
    Vector3 offset = new Vector3(0, 0, -10);
    bool follow;
    
    public void SetCameraTarget(Transform target)
    {
        targetTransform = target;
        StartCameraFollow();
    }

    public void StartCameraFollow()
    {
        follow = true;
    }

    public void StopCameraFollow()
    {
        follow = false;
    }

   
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(follow && targetTransform)
        CameraMovement();
	}

    void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, Time.deltaTime * damping);
    }

}
