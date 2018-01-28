using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ROFRAMObjectGroup  {

	[Tooltip("The string ID that identifies this object group.")]
	public string groupName;
	[Tooltip("If set to true, other instances of this script will be able to use this object group by referencing its name. Be careful! Make sure all static groups have unique names, otherwise, expect errors. Note: If a non-static object group has the same name as a static one, the non-static group will be used instead.")]
	public bool isStatic = false;
	[Tooltip("This is the last of GameObject prefabs that will be randomly chosen and spawned by hardpoints that use this object group.")]
	public List<ROFRAMObject> prefabList = new List<ROFRAMObject>();


	public ROFRAMObjectGroup(string n, bool isStat, List<ROFRAMObject> prefLst) {

		groupName = n;
		isStatic = isStat;
		prefabList = prefLst;

	}

}
