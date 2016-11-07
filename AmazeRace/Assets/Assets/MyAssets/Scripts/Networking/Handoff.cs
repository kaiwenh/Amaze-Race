using UnityEngine;
using System.Collections;

public class Handoff : MonoBehaviour {

	private int recalls = 0;

	// Use this for initialization
	void Start () {

		DontDestroyOnLoad(transform.gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Called by the server.
	public void TellClientsToLoadLevel(int lid)
	{
		networkView.RPC ("TellClientLoadLevel", RPCMode.Others, lid);
		//StartButton.interactable = false;
		Debug.Log("Waiting for clients...");
	}

	[RPC]
	void TellClientLoadLevel(int lid)
	{
		Application.LoadLevel  (lid);
	}

	// WakeUpServer calls the RPC below


	[RPC]
	void OnClientLoadedLevel()
	{
		recalls++;
		Debug.Log ("Recalls = "+recalls);
//		if(recalls == serverClientCount)
//		{
//			Application.LoadLevel (2);
//		}
	}

}
