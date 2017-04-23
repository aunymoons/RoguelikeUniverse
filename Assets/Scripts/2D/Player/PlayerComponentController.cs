using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerComponentController : NetworkBehaviour
{

	// Use this for initialization
	protected virtual void Start () {
        Debug.Log("Works");
        if (!isLocalPlayer) return;
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if (!isLocalPlayer) return;
	}
}
