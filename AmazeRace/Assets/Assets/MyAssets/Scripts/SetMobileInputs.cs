using UnityEngine;
using System.Collections;

public class SetMobileInputs : MonoBehaviour {

	public void InsertMobileInput(int i)
	{
		Globals.mobileInputs |= i;
	}

	void Update()
	{
		if(Input.touchCount == 0)
		{
			Globals.mobileInputs = 0;
		}

	}

}
