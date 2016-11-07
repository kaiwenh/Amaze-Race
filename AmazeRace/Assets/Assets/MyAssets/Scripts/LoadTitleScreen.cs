using UnityEngine;
using System.Collections;

public class LoadTitleScreen : MonoBehaviour {

	private float ennum = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		ennum += Time.deltaTime;

		if(ennum > 2.0f)
			Application.LoadLevel (0);
	
	}
}
