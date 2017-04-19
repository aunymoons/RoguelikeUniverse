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
    }

}
