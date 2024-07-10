using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public Transform holdPoint;
    public Transform cameraHolder;
    private Rigidbody rb;

    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!isLocalPlayer)
        {
            cameraHolder.gameObject.SetActive(false);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        MovePlayer();
        RotateCamera();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = cameraHolder.right * moveX + cameraHolder.forward * moveZ;
        move.y = 0;

        Vector3 newVelocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
        rb.velocity = newVelocity;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
