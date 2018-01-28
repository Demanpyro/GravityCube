using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAroundLobby : MonoBehaviour
{
	public float Speed;
	void Update()
	{

		// ...also rotate around the World's Y axis
		transform.Rotate(0, Speed * Time.deltaTime, 0, Space.World);
	}
}