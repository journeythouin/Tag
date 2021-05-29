using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Controls controls;
    public PlayerMovement movement;
    public MouseLook mouseLook;

    private Vector2 movementInput;
    private float jumpInput;
    Vector2 mouseInput;

    public void Awake()
    {
        controls = new Controls();
        controls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Player.Jump.performed += ctx => jumpInput = ctx.ReadValue<float>();

        controls.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        controls.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void OnEnable() => controls.Enable();

    private void Update()
    {
        movement.ReceiveHorizontalInput(movementInput);
        movement.ReceiveVerticalInput(jumpInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    private void OnDestroy() => controls.Disable();
}
