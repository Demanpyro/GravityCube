using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColor : MonoBehaviour {

    public Material Team_Darkblue;
    public Material Team_Lightblue;
    public Material Team_Orange;
    public Material Team_Red;
    public Material Team_Pink;
    public Material Team_Yellow;

    public bool colorSet;

    public PlayerMovement Player;


    // Use this for initialization
    void Start () {
        colorSet = false;
        print(colorSet);
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!colorSet && Player.gDir != "")
        {
            if (Player.gDir == "Up")
            {
                print("up color");
                GetComponent<SkinnedMeshRenderer>().material = Team_Lightblue;
                print(GetComponent<SkinnedMeshRenderer>().material);
            }
            else if(Player.gDir == "Down")
            {
                print("down color");
                GetComponent<SkinnedMeshRenderer>().material = Team_Darkblue;
                print(GetComponent<SkinnedMeshRenderer>().material);
            }

            colorSet = true;

        }

		
	}
}
