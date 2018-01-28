using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ROFRAMDemoScript : MonoBehaviour {

	public ROFRAM rofRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

		
		if (Input.GetKeyDown(KeyCode.Space)) {
			foreach (GameObject o in rofRef.spawnedObjects) {
				Destroy(o);
				
			}
			rofRef.spawnedObjects = new List<GameObject>();
			rofRef.initialize();
		}

	}
}
