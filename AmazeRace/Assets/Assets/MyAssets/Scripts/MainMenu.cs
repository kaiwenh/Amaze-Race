using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Button JoinServerButton;
	public InputField IPfield;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(IPfield.text == "")
		{
			JoinServerButton.interactable = false;
		}
		else
		{
			JoinServerButton.interactable = true;
		}
	
	}

	public void ConnectToServerAndLoadRoom()
	{
		Globals.hostIP = IPfield.text;
		Application.LoadLevel (1);
		//Network.Connect(IPfield.text, 25000);
	}

	public void StartServerAndLoadRoom()
	{
		Network.InitializeServer(4,25000,false);
	}

	void OnServerInitialized()
	{
		Debug.Log ("Server's up: "+Network.player.ipAddress);
		Application.LoadLevel(1);
	}

	void OnConnectedToServer()
	{
		//Debug.Log ("Successfully connected! "+Network.player.ipAddress);
		//Application.LoadLevel(1);
	}

	void OnFailedToConnect()
	{
		Debug.Log ("Failed to connect :("); 
	}
}
