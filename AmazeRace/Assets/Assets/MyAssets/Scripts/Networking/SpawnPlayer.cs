using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

	public Transform player1;
	public Transform player2;
	public Transform player3;
	public Transform player4;

	public Transform p1Spawn;
	public Transform p2Spawn;
	public Transform p3Spawn;
	public Transform p4Spawn;

	private int serverClientCount = 0;
	private int recalls = 0;

	public Button StartButton;

	public int GetServerClientCount()
	{
		return serverClientCount;
	}

	// Use this for initialization
	void Start () {

		DontDestroyOnLoad(transform.gameObject);

		if(Network.peerType != NetworkPeerType.Disconnected)
		{	
			if(Network.peerType == NetworkPeerType.Server)
			{
				Debug.Log ("Server SpawnPlayer object loaded...");
				SpawnSelf ();
			}
		}
		else // You're a client
		{
			Network.Connect(Globals.hostIP, 25000);
		}
	}

	// Called in lobby setting
	void OnConnectedToServer()
	{
		SpawnSelf ();
	}

//	[RPC]
//	void SpawnYourself()
//	{
//		SpawnSelf ();
//	}

	void SpawnSelf()
	{
		int PC = int.Parse(Network.player.ToString());
		switch(PC)
		{
		case 0:
			Network.Instantiate(player1, p1Spawn.position, player1.rotation, 0);
			break;
		case 1:
			Network.Instantiate(player2, p2Spawn.position, player2.rotation, 0);
			break;
		case 2:
			Network.Instantiate(player3, p3Spawn.position, player3.rotation, 0);
			break;
		case 3:
			Network.Instantiate(player4, p4Spawn.position, player4.rotation, 0);
			break;
		}
	}

	void OnFailedToConnect()
	{
		Debug.Log ("Failed to connected");
		Application.LoadLevel (0);
	}

	// Called in lobby setting
	void OnPlayerConnected()
	{
		serverClientCount++;
		Debug.Log("Server Client Count: "+serverClientCount);
		if(serverClientCount > 0)
			StartButton.interactable = true;
	}

	// Called in lobby setting
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		serverClientCount--;
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);

		if(serverClientCount == 0 && StartButton)
			StartButton.interactable = false;
	}

	void OnDisconnectedFromServer()
	{
		Application.LoadLevel(4);
	}


	void TellClientsWereReady()
	{

	}

	[RPC]
	void OnClientLoadedLevel()
	{
		recalls++;
		Debug.Log ("Recalls = "+recalls);
		if(recalls >= serverClientCount)
		{
			recalls = 0;
			Application.LoadLevel (Globals.nextLevelID);
		}
	}

	public void nonRPCOnClientLoadedLevel()
	{
		recalls++;
		Debug.Log ("Recalls = "+recalls);
		if(recalls >= serverClientCount)
		{
			recalls = 0;
			Debug.Log ("Server now loading level "+Globals.nextLevelID);
			Application.LoadLevel (Globals.nextLevelID);
		}
	}

	public void TellClientsToLoadLevel(int lid)
	{
		Debug.Log (networkView.transform.name);
		Globals.nextLevelID = lid;
		networkView.RPC ("TellClientLoadLevel", RPCMode.Others, lid);
		if(StartButton)
			StartButton.interactable = false;
		Debug.Log("Waiting for clients... for level "+lid);
	}

	[RPC]
	void TellClientLoadLevel(int lid)
	{
		Globals.nextLevelID = lid;
		Application.LoadLevel  (lid);
	}

	public void nonRPCTellClientLoadLevel(int lid)
	{
		Globals.nextLevelID = lid;
		Application.LoadLevel  (lid);
	}

	void OnLevelWasLoaded(int level)
	{
		if(level == 4 || level == 0)
			Destroy (gameObject);
	}

}
