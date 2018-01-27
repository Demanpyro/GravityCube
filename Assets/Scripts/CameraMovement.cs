using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform Player;
    public PlayerMovement dir;
    public float mouseSensitivity = 100.0f;
    public float clampAngleY;
    private float clampUp;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        clampUp = clampAngleY - 90;
    }

    void Update()
    {
        

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        if (dir.gDir == "Down")
        {
            rotY -= mouseX * mouseSensitivity * Time.deltaTime;
            rotX -= mouseY * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;
        }
        

        rotX = Mathf.Clamp(rotX, -clampAngleY + clampUp, clampAngleY);

        Vector3 originalRotation = Player.rotation.eulerAngles;

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, originalRotation.z);
        transform.rotation = localRotation;
        Quaternion playerRotation = Quaternion.Euler(originalRotation.x, rotY, originalRotation.z);
        Player.rotation = playerRotation;
    }
}
