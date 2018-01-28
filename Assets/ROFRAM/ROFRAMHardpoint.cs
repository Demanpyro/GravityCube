using UnityEngine;
using System.Collections;

[System.Serializable]
public class ROFRAMHardpoint  {

	[Tooltip("This string variable is used for both your reference and for searches using the getHardpointByName() function.")]
	public string name;
	[Tooltip("Drag and drop your hardpoint GameObject here--this is the 3D point that will spawn your objects.")]
	public Transform hardPointTransform;
	[Tooltip("The name of the object group that you want to spawn objects from. This name must match one of the object group names above.")]
	public string groupName;
	[Tooltip("By checking this box, all children one level below the parent transform will be used as hardpoints to spawn objects with. Can save time when working with large amounts of hardpoints that use the same group. NOTE: an object will not spawn at the root transform, only its children. If you want an object to spawn at the parent's position, you will need to create another blank GameObject with local position 0,0,0.")]
	public bool useChildrenAsHardpoints = false;
	[Tooltip("If all children are being used as hardpoints, checking this flag will cause the random generator to only choose one object that is then spawned on all of the children. Useful for symmetry.")]
	public bool sameObjectForAllChildren = false;

	[Tooltip("Checking any of these will result in a random rotation value from 0 to 360 to be applied on that axis. The rotation applied is based on the rotation of the parent, not absolute coordinates.")]
	public bool randomXRotation, randomYRotation, randomZRotation;

	[HideInInspector]
	public ROFRAM root; //Link back if necessary, is initialized by the root script.





	public ROFRAMHardpoint(string n, Transform tr, string gN, bool childrenAsHardpoints, bool sameObjForChildren, bool randX, bool randY, bool randZ) {

		name = n;
		hardPointTransform = tr;
		groupName = gN;
		useChildrenAsHardpoints = childrenAsHardpoints;
		sameObjectForAllChildren = sameObjForChildren;
		randomXRotation = randX;
		randomYRotation = randY;
		randomZRotation = randZ;


	}




	//Returns the rotation with random axes taken into account. Used when initialized hardpoints.
	public Quaternion getRotation(Transform tr) {
		Vector3 rotVec = tr.rotation.eulerAngles;

		if (randomXRotation)
			rotVec.x = Random.Range (0, 360);
		if (randomYRotation)
			rotVec.y = Random.Range (0, 360);
		if (randomZRotation)
			rotVec.z = Random.Range (0, 360);


		return Quaternion.Euler(rotVec);


	}


}
