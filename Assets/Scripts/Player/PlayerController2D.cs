using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController2D : MonoBehaviour
{
    [Header("Player Stats")]
    public float moveSpeed;
    public float maxMoveSpeed;
    private Transform playerT;

    public float jumpForce;
    private Rigidbody2D rb;
    private Vector2 playerPos;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}

