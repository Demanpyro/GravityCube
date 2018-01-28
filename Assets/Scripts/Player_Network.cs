using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour {

    public GameObject Player;
    public GameObject[] characterModel;

    public override void OnStartLocalPlayer()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        Player.SetActive(true);

        foreach(GameObject go in characterModel)
        {
            go.SetActive(false);
        }

    }

}
