using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ROFRAMDemoSlider : MonoBehaviour {

	public Slider sl;
	public string objName, objGroup;
	public ROFRAM rof;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		sl.onValueChanged.AddListener (slListener);
	}

	public void slListener(float value)	{

		rof.adjustWeight (value, objName, objGroup);

	}


}
