using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gateway : MonoBehaviour {

	public Text text2display;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartServer()
	{
		Network.InitializeServer(4,25000,false);
		Debug.Log ("clicked");
	}

	void OnServerInitialized()
	{
		Debug.Log ("Server successfully made. Go get a sandwich for yourself :D\nIP address: "+Network.player.ipAddress);
	}
}
