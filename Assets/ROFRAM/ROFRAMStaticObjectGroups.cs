using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("ROFRAM/ROFRAM Static Object Groups")]

//This script will be set up automatically if not found, you don't need to manually add this.

public class ROFRAMStaticObjectGroups : MonoBehaviour {
	
	public static List<ROFRAMObjectGroup> staticObjectGroups = new List<ROFRAMObjectGroup> ();


	void Awake() {

		ROFRAM.statObjRef = this;

	}

	public void clearList() {
		staticObjectGroups.Clear ();

	}

	//Returns the static object group with the specified name.
	public static ROFRAMObjectGroup getStaticObjectGroupByName(string n) {
		
		foreach (ROFRAMObjectGroup g in staticObjectGroups) {
			if (n == g.groupName) {
				return g;
			}
			
		}
		Debug.LogError("(ROFRAM) No static object group with name " + n + " found.");
		return null;
		
	}

	//Adjusts the weight for a static ROFRAMObject.
	public static void adjustWeightStatic(float value, string objectName, string objectGroup) {
		
		getObjectByNameFromStaticGroup (objectName, objectGroup).randomWeight = value;
		
	}


	//Input the name of the ROFRAMObject, and the name of the group it belongs to, and the appropriate static ROFRAMObject will be returned.
	public static ROFRAMObject getObjectByNameFromStaticGroup(string objectName, string groupName) {
		
		if (ROFRAM.statObjRef == null) {
			Debug.LogError ("(ROFRAM) getObjectByNameFromStaticGroup(objectName, groupName) call unsuccessful. No static groups are defined!");
			return null;
		}
		
		foreach (ROFRAMObjectGroup g in staticObjectGroups) {
			if (groupName == g.groupName) {
				foreach (ROFRAMObject rObj in g.prefabList) {
					if (rObj.name == objectName)
						return rObj;
				}
				
			}
			
		}
		
		Debug.LogError ("(ROFRAM) getObjectByNameFromStaticGroup(objectName, groupName) call unsuccessful. Check static object and group name strings.");
		return null;

	}
	

	//Creates a new ROFRAMObject that can be spawned by the specified static group.
	public static void createAndAddObjectToStaticGroup(string groupName, string newObjName, GameObject prefab, float randomWeight) {
		
		ROFRAMObject nRofObj = new ROFRAMObject (newObjName, prefab, randomWeight);
		getStaticObjectGroupByName(groupName).prefabList.Add (nRofObj);
		
	}

}
