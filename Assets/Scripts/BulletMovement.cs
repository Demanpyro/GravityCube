using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BulletMovement : NetworkBehaviour {

    public Rigidbody Player;
    public PlayerMovement dir;
    public float force;
    public float bump;

    public GameObject particles;

    public BulletShot BulletScript;
    public bool isPush;
    public bool isPull;
    public bool goPush;
    public bool goPull;
    public float speed;
    public float maxTime;
    public float timer;


    void Start()
    {
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (BulletScript.Pull && isPull)
        {
            if (maxTime >= timer)
            {
                particles.SetActive(true);
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
                particles.SetActive(true);
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
            if (dir.gDir == "Up")
            {
                Player.velocity = (transform.up * force + (Vector3.up * bump));
            }
            else if(dir.gDir == "Down")
            {
                Player.velocity = (transform.up * force + (-Vector3.up * bump));
            }
            
            
            goPull = false;
        }

        if (goPush)
        {
            if (dir.gDir == "Up")
            {
                Player.velocity = (-transform.up * force + (Vector3.up * bump));
            }
            else if(dir.gDir == "Down")
            {
                Player.velocity = (-transform.up * force + (-Vector3.up * bump));
            }
            goPush = false;
        }

    }

    public void ResetPos()
    {
        particles.SetActive(false);
        transform.localPosition = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Player")
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
