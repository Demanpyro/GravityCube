using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : MonoBehaviour {

    public float speed;
    public bool up;
    public bool begin = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            begin = true;
        }

        if (begin)
        {
            if (up)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            else
                transform.position -= Vector3.up * speed * Time.deltaTime;
        }
        
    }
}
