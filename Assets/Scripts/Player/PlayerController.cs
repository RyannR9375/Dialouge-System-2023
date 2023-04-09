using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    //MOVING
    public float moveSpeed;
    public float groundDrag;
    //JUMPING
    public float jumpForce;
    public float jumpCD;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float extraHeightCheck;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        //GROUND CHECK
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraHeightCheck, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //SET INPUTS EQUAL TO UNITY'S PRE-DESIGNATED BUTTONS
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput   = Input.GetAxisRaw("Vertical");

        //CHECK IF USER PRESSED JUMP BUTTON
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCD);
        }
    }

    private void MovePlayer()
    {
        //CALC MOVEMENT DIRECTION

        //ENSURES YOU'LL ALWAYS WALK IN THE DIRECTION YOU'RE LOOKING
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //ON GROUND
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //IN AIR
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //LIMIT VELOCITY
        if(flatVel.magnitude > moveSpeed) //IF YOU GO FASTER THAN YOUR MOVEMENT SPEED,
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; // CALCULATE WHAT MAX VELOCITY WOULD BE
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // APPLY THIS VALUE TO CURRENT RB.VELOCITY
        }
    }

    private void Jump()
    {
        //RESET Y VELOCITY TO ENSURE JUMPING THE EXACT SAME HEIGHT EVERY TIME
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController2D : MonoBehaviour
{
    [Header("Player Stats")]
    public float moveSpeed;
    public float maxMoveSpeed;

    public float jumpForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float extraHeightCheck;
    public float airMultiplier;
    bool grounded;

    Vector2 moveDirection;

    float horizontalInput;
    float verticalInput;
    bool readyToJump;
    public float jumpCD;

    private Rigidbody2D rb = default;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {

        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //SET INPUTS EQUAL TO UNITY'S PRE-DESIGNATED BUTTONS
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //CHECK IF USER PRESSED JUMP BUTTON
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCD);
        }
    }

    private void MovePlayer()
    {
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode2D.Force);

        //ON GROUND
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode2D.Force);

        //IN AIR
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode2D.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector2(rb.velocity.x, 0f);

        //LIMIT VELOCITY
        if (flatVel.magnitude > moveSpeed) //IF YOU GO FASTER THAN YOUR MOVEMENT SPEED,
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; // CALCULATE WHAT MAX VELOCITY WOULD BE
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // APPLY THIS VALUE TO CURRENT RB.VELOCITY
        }
    }

    private void Jump()
    {
        //RESET Y VELOCITY TO ENSURE JUMPING THE EXACT SAME HEIGHT EVERY TIME
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.CompareTag("whatIsGround") && collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
        }
    }
}

*/