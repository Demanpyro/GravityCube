using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{

    public float mspeed;
    public float rspeed;
    [SerializeField] private float jumpHeight = 5f;
    public bool isGrounded;
    public bool isWall;
    public bool crouch;
    public Transform groundCheck;
    //public Transform wallCheck;

    private Rigidbody rb;

    public Animator anim;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rspeed = mspeed;

    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
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
                    GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight);
                    isGrounded = false;
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("groundhit");
        if (isGrounded)
        {
            Debug.Log("groundhit");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            print(GetComponent<Rigidbody>().velocity);
            print(GetComponent<Rigidbody>().angularVelocity);
        }
    }
}