using UnityEngine;
using System.Collections;

public class SendPlayersToSpawns : MonoBehaviour {

	public Transform[] spawnPoints;

	// Use this for initialization
	void Start () {

		if(Network.peerType == NetworkPeerType.Server)
		{
			MovementTypeA[] avatars = GameObject.FindObjectsOfType<MovementTypeA>();
			for(int i = 0; i < avatars.Length; i++)
			{
				Transform trans = avatars[i].GetComponent<Transform>();
				int playerIndex = int.Parse(trans.networkView.owner.ToString());
				trans.position = spawnPoints[playerIndex].position;
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
