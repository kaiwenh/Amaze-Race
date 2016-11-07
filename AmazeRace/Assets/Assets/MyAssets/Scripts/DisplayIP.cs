using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayIP : MonoBehaviour {

	public Text title;
	public Text IPdisplay;

	// Use this for initialization
	void Start () {

		if(Network.peerType == NetworkPeerType.Server)
		{
			IPdisplay.text = Network.player.ipAddress;
		}
		else
		{
			title.enabled = false;
			IPdisplay.enabled = false;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
