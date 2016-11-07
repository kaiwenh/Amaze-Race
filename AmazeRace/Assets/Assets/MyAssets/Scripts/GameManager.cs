using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent (typeof(NetworkView))]

public class GameManager : MonoBehaviour {

	public Transform Goal;
	public int winCondition = 0; // 1 == same y, 2 == same x
	public float range = 0.1f;

	public Text[] placementGraphics;
	public Canvas NextLevelMenu;

	private MovementTypeA[] avatars;
	private MovementTypeA yourAvatar;
	private List<int> completed;

	private SpawnPlayer SP;


	// Use this for initialization
	void Start () {

		avatars = GameObject.FindObjectsOfType<MovementTypeA>();
		completed = new List<int>();
		SP = GameObject.FindObjectOfType<SpawnPlayer>();
		yourAvatar = NetworkView.Find (Globals.NVID).GetComponent<MovementTypeA>();
	}

	bool inRangeOfGoal(Vector3 position)
	{
		switch (winCondition)
		{
		case 0:
			Vector3 noZ = position - Goal.position;
			noZ = new Vector3(noZ.x, noZ.y, 0.0f);
			return ( noZ.magnitude <= range );
			break;
		case 1:
			return ( Mathf.Abs(position.y - Goal.position.y) <= range);
			break;
		case 2:
			return ( Mathf.Abs(position.x - Goal.position.x) <= range);
			break;
		default:
			return ( Vector3.Distance (position, Goal.position) <= range );
			break;
		}
	}

	bool hasWon(int ownerID)
	{
		return completed.Contains(ownerID);
	}
	
	// Update is called once per frame
	void Update () {

		if(Network.peerType == NetworkPeerType.Server)
		{
			for(int i = 0; i < avatars.Length; i++)
			{
				int ownerInt = int.Parse(avatars[i].networkView.owner.ToString());
				if(	inRangeOfGoal (avatars[i].transform.position) && 
				   	!hasWon(ownerInt)
					)
				{

					if(completed.Count == 0)
						// Give crown :D
						networkView.RPC("RevealWinner", RPCMode.All, avatars[i].gameObject.name);

					completed.Add(ownerInt);

					if(avatars[i].networkView.owner != Network.player)
					// If a client wins...
					{
						Debug.Log ("List's size: "+completed.Count);
						networkView.RPC("RevealPlacement", avatars[i].networkView.owner, completed.Count);
					}
					else // Server won...
						placementGraphics[completed.Count-1].enabled = true;

					if(completed.Capacity >= SP.GetServerClientCount())
					{
						NextLevelMenu.enabled = true;
					}
				}
			}
		}


	}

	[RPC]
	void RevealPlacement(int placing)
	{
		placementGraphics[placing-1].enabled = true;
	}

	[RPC]
	void RevealWinner(string p)
	{
		for(int i = 0; i < avatars.Length; i++)
		{
			if(avatars[i].gameObject.name == p)
			{
				Debug.Log ("Found them. Player objects match: " + p);
				avatars[i].WearCrown();
			}
			else
				avatars[i].RemoveCrown();
		}
	}

	[RPC]
	void TellClientLoadLevel(int lid)
	{
		SP.nonRPCTellClientLoadLevel(lid);
	}

	[RPC]
	public void OnClientLoadedLevel()
	{
		SP.nonRPCOnClientLoadedLevel();
	}
}
