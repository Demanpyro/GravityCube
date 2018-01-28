using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{

    public float mspeed;
    public float rspeed;
    [SerializeField] private float jumpHeight = 5f;
    public bool isGrounded;
    public bool isWall;
    public bool crouch;
    public Transform groundCheck;
    public CameraMovement cameraScale;
    //public Transform wallCheck;

    private Rigidbody rb;

    public Animator anim;
    public BulletMovement bullet;
    public Camera cam;

    public Text myText;

    [SyncVar]
    public int hitType;

    public string gDir = "";

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = isLocalPlayer;
        rspeed = mspeed;

    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            cam.enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Screen.lockCursor = true;
            }
            else
            {
                Screen.lockCursor = false;
                Cursor.visible = true;
            }


        }

    //isWall = Physics.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Wall"));
    isGrounded = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            transform.position += transform.forward * Time.deltaTime * mspeed;
            anim.SetFloat("mspeed", .1f);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * mspeed;
            anim.SetFloat("mspeed", -.1f);
        }



        if ((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            anim.SetFloat("mspeed", 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * rspeed;
            anim.SetFloat("sspeed", .1f);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * rspeed;
            anim.SetFloat("sspeed", -.1f);
        }



        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            anim.SetFloat("sspeed", 0f);
        }



        if (isGrounded)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!crouch)
                {
                    //print("jump");
                    if (gDir == "Up")
                    {
                        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight);
                    }
                    else if(gDir == "Down")
                    {
                        GetComponent<Rigidbody>().AddForce(-Vector3.up * jumpHeight);
                    }
                    isGrounded = false;
                }
            }
        }
    }

    [Command]
    void CmdMovePlayer()
    {
        RpcMovePlayer();
    }

    [ClientRpc]
    void RpcMovePlayer()
    {
        if (isLocalPlayer)
        {
            if(hitType == 0)
            {
                GetComponent<Rigidbody>().velocity = (-transform.up * bullet.force + (Vector3.up * bullet.bump));
            }
            else if(hitType == 1)
            {
                GetComponent<Rigidbody>().velocity = (transform.up * bullet.force + (Vector3.up * bullet.bump));
            }

            hitType = 3;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!hasAuthority)
            return;
        Debug.Log("groundhit");
        if (isGrounded)
        {
            Debug.Log("groundhit");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

        void OnTriggerEnter(Collider col)
    {
        if (!hasAuthority)
            return;

        if (gDir == "")
        {
            if (col.gameObject.tag == "Down")
            {
                gDir = "Down";
                transform.localRotation = Quaternion.Euler(0, 0, 180);
                Physics.gravity = new Vector3(0, 9.8f, 0);
            }
            else if (col.gameObject.tag == "Up")
            {
                gDir = "Up";
            }
        }

        if(col.gameObject.tag == "Pull")
        {
            hitType = 0;

            CmdMovePlayer();
        }
        if (col.gameObject.tag == "Push")
        {
            hitType = 1;
            CmdMovePlayer();
        }

        if(col.gameObject.tag == "Lava")
        {
            myText.text = "You Lose!";

        }
    }
}