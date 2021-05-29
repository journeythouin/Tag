using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform groundCheck;
    [SerializeField] LayerMask GroundMask;
    public Collider coll;
    [SerializeField] int groundMask = 7;

    private Vector2 moveInput;
    private float jumpInput;
    private bool isGrounded;
    private Vector3 moveDirection;
    private float backwardsMultiplier;
    private float sidewaysMultiplier;
    private float maxSpeed;
    private float jumpMultiplier;
    private bool canJump = true;

    [SerializeField] private float moveAcceleration = 11f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private float maxSpeedVariable = 5f;
    [SerializeField] private float airDrag = 0.05f;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (moveInput.y == 0 && moveInput.x == 0)
        {
            rb.drag = 1f;
        }
        else rb.drag = 0f;
        if (rb.velocity.y > 0)
        {
            jumpMultiplier = 0f;
        }
        else jumpMultiplier = 1f;

        if (moveInput.y < 0)
        {
            backwardsMultiplier = 0.4f;
        }
        else backwardsMultiplier = 1f;
        if (moveInput.y == 0 && moveInput.x != 0)
        {
            sidewaysMultiplier = 0.75f;
        }
        else sidewaysMultiplier = 1f;
        maxSpeed = maxSpeedVariable * backwardsMultiplier * sidewaysMultiplier;
        moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        Debug.DrawRay(groundCheck.transform.position + transform.forward / 4, Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position - transform.forward / 4, Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position + transform.right / 4, Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position - transform.right / 4, Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position + (transform.forward / 4 / 1.4142f) + (transform.right / 4 / 1.4142f), Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position - (transform.forward / 4 / 1.4142f) + (transform.right / 4 / 1.4142f), Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position + (transform.forward / 4 / 1.4142f) - (transform.right / 4 / 1.4142f), Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);
        Debug.DrawRay(groundCheck.transform.position - (transform.forward / 4 / 1.4142f) - (transform.right / 4 / 1.4142f), Vector3.down, Color.blue, coll.bounds.extents.y + 0.1f);

        if (Physics.Raycast(groundCheck.transform.position + transform.forward / 4, Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position - transform.forward / 4, Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position + transform.right / 4, Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position - transform.right / 4, Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position + (transform.forward / 4 / 1.4142f) + (transform.right / 4 / 1.4142f), Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position - (transform.forward / 4 / 1.4142f) + (transform.right / 4 / 1.4142f), Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position + (transform.forward / 4 / 1.4142f) - (transform.right / 4 / 1.4142f), Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask)
            || Physics.Raycast(groundCheck.transform.position - (transform.forward / 4 / 1.4142f) - (transform.right / 4 / 1.4142f), Vector3.down, coll.bounds.extents.y + 0.1f, ~groundMask))
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    private void FixedUpdate()
    {
        Move();
        if (canJump)
        {
            if (isGrounded && jumpInput > 0)
            {
                StartCoroutine("Jump");
            }
        }
    }

    private void Move()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            if (isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveAcceleration, ForceMode.Acceleration);
            }
            else rb.AddForce(moveDirection.normalized * moveAcceleration * airDrag, ForceMode.Acceleration);
        }   
    }

    IEnumerator Jump()
    {
        canJump = true;

        rb.AddForce(transform.up * (jumpForce * jumpMultiplier), ForceMode.Acceleration);

        if (isGrounded)
        {
            canJump = false;

            if (rb.velocity.magnitude < 1f)
            {
                yield return new WaitForSeconds(1f);
            }
            canJump = true;
        }
    }

    public void ReceiveHorizontalInput(Vector2 _moveInput)
    {
        moveInput = _moveInput;
    }

    public void ReceiveVerticalInput(float _jumpInput)
    {
        jumpInput = _jumpInput;
    }
}
