using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    [Header("Movement Variable")]
    public float moveSpeed = 4f;
    public float jumpForce = 4f;
    public float distToGround = 1f;

    private Vector2 moveInput;

    // Variable for Jumping
    [Header("Jump Variable")]
    public LayerMask WhatIsGround;
    public Transform groundPoint;
    private bool isGrounded = false;

    // Debug Text Variable
    [Header("Debugging")]
    public Text GroundDebug;
    public Text SlopeDebug;

    // In-Script Variable
    float horz;
    float vert;
    bool _jump;
    float _slopeAngle;
    Vector3 deltaPosition;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        deltaPosition = ((transform.forward * vert) + (transform.right * horz)) * moveSpeed * Time.fixedDeltaTime;

        float normalisedSlope = (_slopeAngle / 90f) * -1f;
        deltaPosition += (deltaPosition * normalisedSlope);

        rb.MovePosition(rb.position + deltaPosition);

        if (_jump)
        {
            rb.velocity += (Vector3.up * jumpForce);
            _jump = false;
        }
        GroundCheck();
    }
    void GetInput()
    {
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) _jump = true;
    }

    void GroundCheck()
    {
        // Check if we are on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
        {
            _slopeAngle = (Vector3.Angle(hit.normal, transform.forward) - 90);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        // GroundDebug.text = "Grounded: " + isGrounded;
        GroundDebug.text = "Grounded: " + isGrounded;
    }
}
