using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlatform : NetworkBehaviour {

	public GameObject objectToSpawn;
	public Transform SpawnPoint;


    void Start()
    {
        CmdSpawnPlatform();
    }

    void Update()
	{
		if(!isLocalPlayer)
		{return; }

	}

	[Command]
	void CmdSpawnPlatform()
	{
		GameObject go = Instantiate(objectToSpawn, SpawnPoint);
		NetworkServer.Spawn(go);
		Debug.Log("Spawned Platform");
	}
}
