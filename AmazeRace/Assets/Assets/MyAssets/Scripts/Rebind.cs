using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rebind : MonoBehaviour {

	public Button aButton;
	private SpawnPlayer SP;
	public int nextLevel = 3;

	// Use this for initialization
	void Start () {

		SP = GameObject.FindObjectOfType<SpawnPlayer>();

		if(aButton && SP)
		{
			//aButton.onClick.RemoveAllListeners();
			// Lambdas are da shit.
			aButton.onClick.AddListener( () => Execute() );
		}
	}

	public void Execute()
	{
		SP.TellClientsToLoadLevel(nextLevel);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
