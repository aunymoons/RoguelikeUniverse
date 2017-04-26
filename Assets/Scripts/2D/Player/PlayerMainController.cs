using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMainController : PlayerComponentController
{

    //Prefabs
    WorldController worldController;
    public GameObject cameraPrefab;



    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        if (!isLocalPlayer) return;

        if (!worldController) worldController = FindObjectOfType<WorldController>(); ;

        InstantiateCamera();
        SetTargetWorld();
}

    void InstantiateCamera()
    {
        GameObject camera = Instantiate(cameraPrefab) as GameObject;
        camera.GetComponent<PlayerCameraController>().SetCameraTarget(this.transform);
    }

    void SetTargetWorld()
    {
        worldController.SetTarget(this.transform);
    }


}
