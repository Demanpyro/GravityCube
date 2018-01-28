using UnityEngine;
using System.Collections;

[System.Serializable]
public class ROFRAMObject {
	[Header ("Object")]
	[Tooltip("This string variable is only for your reference and ease of use, it has no effect anywhere else.")]
	public string name;
	[Tooltip("Drag the prefab you want as a candidate for random spawning into this slot.")]
	public GameObject prefab;
	[Range (1.0f, 100.0f)]
	[Tooltip("The higher this value is, the more likely the object is to spawn compared to objects with lower values. A value of 0 means this object has the base chance of spawning (EX: If there are 3 objects all with a base of 0, each object has a 33.3% chance of spawning.). A value of 100 means this object has a 100% chance of spawning if all other objects are set to 0.")]
	public float randomWeight = 1;


	public ROFRAMObject (string objName, GameObject prefObj, float weight) {

		name = objName;
		prefab = prefObj;
		randomWeight = Mathf.Clamp (weight, 1.0f, 100.0f);

	}


}
