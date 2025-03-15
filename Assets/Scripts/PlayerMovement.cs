using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    [Tooltip("The player's rigidbody component")]
    private Rigidbody rb;

    [Header("Player Movement")]
    [Tooltip("The speed at which the player moves")]
    public float moveSpeed;
    [Tooltip("The direction the player is moving in")]
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        // Get the player's input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the player's movement direction
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Move the player
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }
}
