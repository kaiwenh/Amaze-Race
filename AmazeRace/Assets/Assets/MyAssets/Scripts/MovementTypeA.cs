using UnityEngine;
using System.Collections;


[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(CircleCollider2D))]
[RequireComponent (typeof(NetworkView))]

public class MovementTypeA : MonoBehaviour {

	public float speed = 2.5f;
	public SpriteRenderer crown;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
		if(networkView.isMine)
		{
			Globals.NVID = networkView.viewID;
		}
	}

	public void WearCrown()
	{
		Debug.Log ("I want to wear this");
		crown.enabled = true;
	}

	public void RemoveCrown()
	{
		crown.enabled = false;
	}

	int GetUserInputs()
	{
		int inputs = 0;
		if(Input.GetKey (KeyCode.W))
			inputs |= 0x00000001;
		if(Input.GetKey (KeyCode.A))
			inputs |= 0x00000002;
		if(Input.GetKey (KeyCode.S))
			inputs |= 0x00000004;
		if(Input.GetKey (KeyCode.D))
			inputs |= 0x00000008;

		if(Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);
			Vector2 pixelPosition = t.position;

			if(pixelPosition.x < Screen.width*0.25f) // Left
				inputs |= 0x00000002;
			if(pixelPosition.x > Screen.width*0.75f) // RIGHT
				inputs |= 0x00000008;
			if(pixelPosition.y > Screen.height*0.75f) // UP
				inputs |= 0x00000001;
			if(pixelPosition.y < Screen.height*0.25f) // DOWN
				inputs |= 0x00000004;
		}

		return inputs;
	}
	
	Vector2 GetDirection(int inputs)
	{
		Vector2 direction = Vector2.zero;
		if((inputs & 0x00000001)!=0)
			direction += Vector2.up;
		if((inputs & 0x00000002)!=0)
			direction -= Vector2.right;
		if((inputs & 0x00000004)!=0)
			direction -= Vector2.up;
		if((inputs & 0x00000008)!=0)
			direction += Vector2.right;
		direction.Normalize();
		return direction;
	}

	private int localInputBuffer;
	private int remoteInputs;

	private float inputEnnum = 0.0f;
	public float period = 0.025f; //25 ms

	void BufferInputs(int inputs)
	{
		localInputBuffer |= inputs;
	}

	void PeriodicallySendInputInputBuffer()
	{
		inputEnnum += Time.deltaTime;
		if(inputEnnum > period)
		{
			networkView.RPC ("SetRemoteInputs", RPCMode.All, localInputBuffer);
			inputEnnum = 0.0f;
			localInputBuffer = 0;
		}
	}

	// Calls this on all other clients, server, and self
	[RPC]
	void SetRemoteInputs(int inputs)
	{
		remoteInputs = inputs;
	}

	private Vector2 truePosition;
	private float positionEnnum;

	// Only performed by the server. Tells all clients
	// where each player should be.
	void PeriodicallySendTruePosition()
	{
		positionEnnum += Time.deltaTime;
		if(positionEnnum > period)
		{
			Vector2 truePos = 
				new Vector2(transform.position.x, transform.position.y) 
				+ Time.deltaTime*rigidbody2D.velocity;
			networkView.RPC (
				"SetTruePosition", RPCMode.All, truePos.x, truePos.y
				);
			positionEnnum = 0.0f;
		}
	}

	[RPC]
	void SetTruePosition(float x, float y)
	{
		truePosition = new Vector2(x,y);
	}
	
	void MoveGivenInputs(int inputs)
	{
		rigidbody2D.velocity = speed*GetDirection( inputs );
	}
	
	// Update is called once per frame
	void Update () {

		if(Network.peerType != NetworkPeerType.Disconnected)
		{
			if(networkView.isMine)	// ALL users do this with their own instances.
			{
				BufferInputs (GetUserInputs());
				PeriodicallySendInputInputBuffer();
			}

			if(Network.peerType == NetworkPeerType.Server)
			{
				MoveGivenInputs(remoteInputs);
				PeriodicallySendTruePosition();

			}
			else // You're a client...
			{
				// NOTE: rigidbody2D.position is a piece of shit and only visually updates on collision.
				transform.position = new Vector3( truePosition.x, truePosition.y, 0.0f);
				//Debug.Log("Current pos: " + rigidbody2D.position + " True Position: " + truePosition);
			}

		}



	}

	void OnLevelWasLoaded(int level)
	{
		if(level == 4)
			Destroy (gameObject);
	}
}
