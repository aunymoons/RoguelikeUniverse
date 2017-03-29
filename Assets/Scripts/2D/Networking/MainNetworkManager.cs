using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class MainNetworkManager : NetworkManager {

	// Use this for initialization
	void Start () {
		
	}

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        PlayerCameraController cam = FindObjectOfType<PlayerCameraController>();

        List<PlayerMovementController> players = new List<PlayerMovementController>();
        players.AddRange(FindObjectsOfType<PlayerMovementController>());

        foreach(PlayerMovementController player in players)
        {
            if (player.hasAuthority)
            {
                cam.targetTransform = player.transform;
                FindObjectOfType<TileManager>().targetPlayer = player.transform;
            }
        }

        cam.follow = true;
    }

}
