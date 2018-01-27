using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{

    public MeshRenderer PushRender;
    public BoxCollider PushCollide;

    public MeshRenderer PullRender;
    public BoxCollider PullCollide;

    public bool Push;
    public bool Pull;

    void Start()
    {
        Push = false;
        Pull = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !Pull && !Push)
        {
            Debug.Log("Push!");
            PushRender.enabled = true;
            PushCollide.enabled = true;
            Push = true;
        }

        if (Input.GetMouseButtonDown(1) && !Pull && !Push)
        {
            Debug.Log("Pull!");
            PullRender.enabled = true;
            PullCollide.enabled = true;
            Pull = true;

        }
    }
}
