using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] public float sensitivityX = 8f;
    [SerializeField] public float sensitivityY = 8f;
    float mouseOutputX, mouseOutputY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, mouseOutputX * Time.deltaTime);

        xRotation -= mouseOutputY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseOutputX = mouseInput.x * sensitivityX;
        mouseOutputY = mouseInput.y * sensitivityY;
    }
}
