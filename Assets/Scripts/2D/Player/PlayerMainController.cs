using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMainController : NetworkBehaviour {

    //Prefabs
    WorldController worldController;
    public GameObject cameraPrefab;

	// Use this for initialization
	void Start () {

        if (!worldController) worldController = FindObjectOfType<WorldController>();;

        if (hasAuthority)
        {
            InstantiateCamera();
            SetTargetWorld();
        }

	}

    void SetTargetWorld()
    {
        worldController.SetTarget(this.transform);
    }

    void InstantiateCamera()
    {
        GameObject camera = Instantiate(cameraPrefab) as GameObject;
        camera.GetComponent<PlayerCameraController>().SetCameraTarget(this.transform);
    }
}
