using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class InputManager : MonoBehaviour
{



    //States
    public bool editMode;

    //Camera controls
    public float minDistance;
    public float maxDistance;
    public float startDistance;

    // Use this for initialization
    void Start()
    {
        //editMode = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void LateUpdate()
    {
        CameraHandler();
    }

    void CameraHandler()
    {
        if (editMode)
        {
            // Does the main camera exist?
            if (Camera.main != null)
            {
                // Get the world delta of all the fingers
                Vector3 worldDelta = LeanTouch.GetDeltaWorldPosition(10f); // Distance doesn't matter with an orthographic camera

                Camera.main.transform.position -= worldDelta;

                // Make sure the pinch scale is valid
                if (LeanTouch.PinchScale > 0.0f)
                {
                    // Store the old FOV in a temp variable
                    float cameraDistance = Camera.main.transform.position.z;

                    // Scale the FOV based on the pinch scale
                    cameraDistance /= LeanTouch.PinchScale;

                    // Clamp the FOV to out min/max values
                    cameraDistance = Mathf.Clamp(cameraDistance, maxDistance, minDistance);

                    // Set the new FOV
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, cameraDistance);
                }
            }
        }
    }
}
