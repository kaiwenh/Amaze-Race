using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {

	//public float speed = 2.0f;

	// Use this for initialization

	public void Move(Vector2 vel)
	{
		rigidbody2D.velocity = vel;
	}
}
