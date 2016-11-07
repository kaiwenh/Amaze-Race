using UnityEngine;
using System.Collections;

public class StickCameraToPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetworkView mine = NetworkView.Find (Globals.NVID);
		if(mine)
		{
			transform.parent = mine.transform;
			transform.localPosition = new Vector3(0.0f, 0.0f, transform.localPosition.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
