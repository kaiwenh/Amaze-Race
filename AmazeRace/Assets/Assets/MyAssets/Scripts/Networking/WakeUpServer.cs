using UnityEngine;
using System.Collections;

public class WakeUpServer : MonoBehaviour {

	private SpawnPlayer SP;
	private Handoff HO;

	// Use this for initialization
	void Start () {
		SP = GameObject.FindObjectOfType<SpawnPlayer>();

		if(SP)
		{
			if(Network.peerType == NetworkPeerType.Client)
				SP.networkView.RPC ("OnClientLoadedLevel", RPCMode.Server);

			// Server will ALWAYS load after the clients..
			//if(Network.peerType == NetworkPeerType.Server)
			//	SP.networkView.RPC ("SpawnYourself", RPCMode.All);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
