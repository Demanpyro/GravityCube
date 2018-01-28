using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Terra Added this.
using UnityEngine.Networking;

[AddComponentMenu("ROFRAM/ROFRAM Core Script")]
public class ROFRAM : MonoBehaviour {


	[Header("Randomized Object Framework Core Script")]
	[Space(10)]
	[Tooltip("Check this if you don't want the object to spawn random members as soon as it comes into play (By using the Start() function). Instead, you will need to call initialize() to set this object up.")]
	public bool delayInitialization = false;
	[Header("---------------------------")]
	[Space(10)]
	[Tooltip("Define object groups here. These object groups represent separate containers of multiple prefabs that hardpoints can be assigned to choose from. Having one object in a group is perfectly fine.")]
	public List<ROFRAMObjectGroup> objectGroups = new List<ROFRAMObjectGroup>();
	[Space(5)]
	[Header("---------------------------")]
	[Space(5)]
	[Tooltip("Define hardpoints here. Hardpoints take two parameters: a transform (the coordinates of the hardpoint) and the name of the object group it will spawn prefabs from.")]
	public List<ROFRAMHardpoint> hardpoints = new List<ROFRAMHardpoint>();
	[HideInInspector]
	public List<GameObject> spawnedObjects = new List<GameObject>(); //List of all objects this script is directly responsible for spawning.
	
	public static ROFRAMStaticObjectGroups statObjRef; //A reference to the static object group script.

	void Awake() {

		//If we don't have an initialized static object script, create one.
		if (statObjRef == null) {
			GameObject obj = new GameObject();
			obj.name = "ROFRAM Static Object Groups Container";
			obj.AddComponent<ROFRAMStaticObjectGroups>();

		}

		//Set up the static object groups.
		foreach (ROFRAMObjectGroup g in objectGroups) {
			if (g.isStatic) {
				if (!ROFRAMStaticObjectGroups.staticObjectGroups.Contains(g)) {
					ROFRAMStaticObjectGroups.staticObjectGroups.Add(g);
				} else {
					Debug.LogWarning("(ROFRAM) Duplicate object group in the static object group list.");

				}

			}

		}

	}

	// Use this for initialization
	void Start () {

		if (!delayInitialization && hardpoints.Count > 0)
			initialize ();
	

	}

	//Call this to set up the entire ROFRAM spawner.
	public void initialize() {

		foreach (ROFRAMHardpoint h in hardpoints) {
			initializeHardpoint(h);
		}

	}


	private void initializeHardpoint(ROFRAMHardpoint h) {

		//Let the hardpoint know which ROFRAM spawner it belongs to.
		h.root = this;

		//If we're only spawning an object at one hardpoint...
		if (!h.useChildrenAsHardpoints) {
			GameObject obj = (GameObject) Instantiate(getRandomObjectFromGroup(h.groupName), h.hardPointTransform.position, h.getRotation(h.hardPointTransform));
			obj.transform.parent = transform;
			spawnedObjects.Add(obj);

		//Otherwise, we're spawning on this hardpoint's children.
		} else {
			if (!h.sameObjectForAllChildren) {
				//Choose a different object for each child hardpoint.
				foreach(Transform tr in h.hardPointTransform) {
					GameObject obj = (GameObject) Instantiate(getRandomObjectFromGroup(h.groupName), tr.position, h.getRotation(tr));
					obj.transform.localScale = tr.localScale;
					obj.transform.parent = tr;
					spawnedObjects.Add(obj);
				}
			} else {
				//Choose one object, and spawn it on all the children hardpoints.
				GameObject selectedObject = getRandomObjectFromGroup(h.groupName);
				foreach(Transform tr in h.hardPointTransform) {
					GameObject obj = (GameObject) Instantiate(selectedObject, tr.position, h.getRotation(tr));
					obj.transform.localScale = tr.localScale;	
					obj.transform.parent = tr;
					spawnedObjects.Add(obj);
				}
				
			}
			
		}

	}


	//Logic for choosing a random object from the group we want.
	public GameObject getRandomObjectFromGroup(string groupName) {
	
		//Checks the local object group for this instance.
		foreach (ROFRAMObjectGroup g in objectGroups) {
			if (groupName == g.groupName) {
			
				int index = getWeightedRandomIndex(g);

				if (g.prefabList[index].prefab != null) 
					return g.prefabList[index].prefab;
				else {
					Debug.LogError("(ROFRAM) Non-existent object detected in local object group: " + g.groupName + " on Game Object: " + gameObject.name);
					return new GameObject();
				}

			}

		}
	
		//If there's no matches above, we'll check the static object groups.
		foreach (ROFRAMObjectGroup g in ROFRAMStaticObjectGroups.staticObjectGroups) {

			if (groupName == g.groupName) {

				int index = getWeightedRandomIndex(g);

				if (g.prefabList[index].prefab != null) 
					return g.prefabList[index].prefab;
				else {
					Debug.LogError("(ROFRAM) Non-existent object detected in static object group: " + g.groupName + " on Game Object: " + gameObject.name);
					return new GameObject();
				}
				
			}

		}
	
		//If no name matches are found, then there's an issue somewhere. We'll spawn a blank gameobject and log an error so things don't annihilate the project for some reason.

		Debug.LogError("(ROFRAM) Object group with name " + groupName + " not set up properly and failed to spawn. GameObject name: " + gameObject.name);
		return new GameObject();
	
	}

	//Internal function used to determine which object to spawn based on random weighting.
	private int getWeightedRandomIndex(ROFRAMObjectGroup group) {

		float weightTotal = 0;
		List<float> weights = new List<float> ();

		//Add each weight to the list, total weight in the list is calculated when loop exits.
		foreach(ROFRAMObject obj in group.prefabList) {

			//Ensures random weights are at least 1.0f, otherwise stuff will break.
			if (obj.randomWeight <= 0.0f)
				obj.randomWeight = 1.0f;

			weights.Add(obj.randomWeight);
			weightTotal += obj.randomWeight;

		}

		//Normalize all the weights between 0f-1f
		for (int i = 0; i < weights.Count; i++) 
			weights[i] = weights[i] / weightTotal;

		//Spits out the random index based on the weight. If something goes wrong for some reason, we return the index 0.
		float rnd = Random.Range(0.0f,1.0f);
		float aggregateWeight = 0.0f;
		for (int i = 0; i < weights.Count; i++) {
			aggregateWeight += weights[i];

			if (rnd < aggregateWeight)
				return i;

		}
		Debug.LogWarning ("(ROFRAM) Something probably went wrong, check the getWeightedRandomIndex function, and ensure random weights are above 1.0f.");
		return 0;

	}

	//Input the name of the hardpoint, and the ROFRAMHardpoint object will be returned if there is a match.
	public ROFRAMHardpoint getHardpointByName(string n) {

		foreach (ROFRAMHardpoint h in hardpoints) {
			if (h.name == null || h.name == "") {
				continue;

			}
			if (h.name == n) {
				return h;

			}

		}

		Debug.LogError ("(ROFRAM) Hardpoint name " + n + " not found on Game Object " + gameObject.name);
		return null;

	}


	//Input the name of the ROFRAMObject, and the name of the group it belongs to, and the appropriate ROFRAMObject will be returned.
	public ROFRAMObject getObjectByNameFromGroup(string objectName, string groupName) {

		foreach (ROFRAMObjectGroup g in objectGroups) {
			if (groupName == g.groupName) {
				foreach (ROFRAMObject rObj in g.prefabList) {
					if (rObj.name == objectName)
						return rObj;
				}
		
			}
			
		}

		Debug.LogError ("(ROFRAM) getObjectByNameFromGroup(objectName, groupName) call unsuccessful. Check object and group name strings.");
		return null;


	}



	//Allows you to create a hardpoint through a script. Will not automatically spawn an object unless this is created before this script spawns all objects.
	//Example of usage post-initialization:
	//createHardpoint ("NewHardpoint", new GameObject ().transform, "Shapes");
	//spawnObjectAtHardpoint ("NewHardpoint");
	public void createHardpoint(string name, Transform pointTrans, string groupName, bool useChildrenAsHardpoints, bool allChildrenSameObject, bool randX, bool randY, bool randZ) {

		ROFRAMHardpoint newHardpoint = new ROFRAMHardpoint (name, pointTrans, groupName, useChildrenAsHardpoints, allChildrenSameObject, randX, randY, randZ);
		newHardpoint.root = this;
		hardpoints.Add (newHardpoint);


	}

	//Same function as above but with fewer parameters to speed things up if you don't need them.
	public void createHardpoint(string name, Transform pointTrans, string groupName) {

		createHardpoint (name, pointTrans, groupName, false, false, false, false, false);

	}
	
	//Will spawn an object on the intended hardpoint, even if one has already been spawned.

	public bool spawnObjectAtHardpoint(string hardpointName) {


		ROFRAMHardpoint h;
		
		if ((h = getHardpointByName (hardpointName)) != null) {
			initializeHardpoint (h);
			return true;

		}

		Debug.LogError("(ROFRAM) Error spawning hardpoint: hardpoint name " + hardpointName + " does not exist!");
		return false;


	}

	//Returns the object group that matches this name.
	public ROFRAMObjectGroup getObjectGroupByName(string n) {

		foreach (ROFRAMObjectGroup g in objectGroups) {
			if (n == g.groupName) {
				return g;
			}
			
		}
		Debug.LogError("(ROFRAM) No object group with name " + n + " found.");
		return null;

	}

	//Creates a new ROFRAMObject that can be spawned by the specified group.
	//Specify the name of the object group, the name of the object you want to add, and then pass the game object and weight.
	public void createAndAddObjectToGroup(string groupName, string newObjName, GameObject prefab, float randomWeight) {

		ROFRAMObject nRofObj = new ROFRAMObject (newObjName, prefab, randomWeight);
		getObjectGroupByName (groupName).prefabList.Add (nRofObj);


	}

	//Adjusts the weight for a ROFRAMObject.
	//Values = weight, objectName = the name of the object whose weight you want to adjust, objectGroup = the group that object belongs to.
	public void adjustWeight(float value, string objectName, string objectGroup) {

		getObjectByNameFromGroup (objectName, objectGroup).randomWeight = value;

	}

	//Creates a new ROFRAM spawner by inputting the proper object groups and hardpoints. Needs to be added to a game object.
	//Ex: ROFRAM rofObj = (ROFRAM) gameObject.AddComponent<ROFRAM>();
	//rofObj = createNewROFRAMSpawner(parameters);
	public ROFRAM createNewROFRAMSpawner(List<ROFRAMObjectGroup> objGroups, List<ROFRAMHardpoint> hardpts, bool initializeImmediately) {
	
		ROFRAM newRof = new ROFRAM();
		
		newRof.objectGroups = objGroups;
		newRof.hardpoints = hardpts;
		newRof.delayInitialization = !initializeImmediately;

		return newRof;
	
	}



}
