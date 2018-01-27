using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletMovement : MonoBehaviour {

    public Rigidbody Player;
    public float force;
    public float bump;

    public Transform angle;

    public BulletShot BulletScript;
    public bool isPush;
    public bool isPull;
    public bool goPush;
    public bool goPull;
    public float speed;
    public float maxTime;
    public float timer;

	
	// Update is called once per frame
	void Update () {

        if (BulletScript.Pull && isPull)
        {
            if (maxTime >= timer)
            {
                transform.position += transform.up * Time.deltaTime * speed;
                timer += Time.deltaTime;
            }
            else
            {
                ResetPos();
                BulletScript.Pull = false;
                timer = 0;
            }

        }

        if (BulletScript.Push && isPush)
        {
            if (maxTime >= timer)
            {
                transform.position += transform.up * Time.deltaTime * speed;
                timer += Time.deltaTime;
            }
            else
            {
                ResetPos();
                BulletScript.Push = false;
                timer = 0;
            }
        }

    }

    void FixedUpdate()
    {
        if (goPull)
        {
            Player.velocity = (transform.up * force + (Vector3.up * bump));
            
            
            goPull = false;
        }

        if (goPush)
        {
            Player.velocity = (-transform.up * force + (Vector3.up * bump));
            goPush = false;
        }

    }

    public void ResetPos()
    {
        transform.localPosition = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (isPull)
            {
                Rigidbody Oplayer = col.GetComponent<Rigidbody>();
                Oplayer.velocity = (-transform.up * force + (Vector3.up * bump));
                BulletScript.Pull = false;
            }

            if (isPush)
            {
                Rigidbody Oplayer = col.GetComponent<Rigidbody>();
                Oplayer.velocity = (transform.up * force + (Vector3.up * bump));
                BulletScript.Push = false;
            }

        }
        else
        {
            if (isPull)
            {
                goPull = true;
                BulletScript.Pull = false;
            }

            if (isPush)
            {
                goPush = true;
                BulletScript.Push = false;
            }
        }

        ResetPos();
    }
}
