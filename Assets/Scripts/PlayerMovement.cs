using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField] private int mspeed;
    [SerializeField] private float jumpHeight = 5f;
    public bool isGrounded;
    public bool isWall;
    public bool crouch;
    public Transform groundCheck;
    //public Transform wallCheck;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody>();

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
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * mspeed;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * mspeed;
            //transform.Rotate(Vector3.up * Time.deltaTime * rspeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * mspeed;
            //transform.Rotate(-Vector3.up * Time.deltaTime * rspeed);
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
}