using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorByPeerType : MonoBehaviour {
	
	public Image icon;

	public Color offlineColor;
	public Color hostColor;
	public Color clientColor;
	public Color connectingColor;

	void Start()
	{
		icon.color = offlineColor;
	}

	void OnServerInitialized()
	{
		if(Network.peerType == NetworkPeerType.Server)
			icon.color = hostColor;
	}

	void OnConnectedToServer()
	{
		if(Network.peerType == NetworkPeerType.Client)
			icon.color = clientColor;
	}

	void OnDisconnectedFromServer()
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
			icon.color = offlineColor;
	}
}
