using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(CircleCollider2D))]
public class BasicController : MonoBehaviour {

	public float speed = 2.0f;

	// Use this for initialization
	void Start () {
	
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

	void MoveGivenInputs(int inputs)
	{
		rigidbody2D.velocity = speed*GetDirection( inputs );
	}

	// Update is called once per frame
	void Update () {

		if(rigidbody2D)
		{
			rigidbody2D.velocity = speed*GetDirection ( GetUserInputs() );
		}
	
	}
}
