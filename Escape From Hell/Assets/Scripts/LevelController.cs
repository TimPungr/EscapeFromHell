using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // movement
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f;
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;


    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        if (_controller.isGrounded)
            _velocity.y = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            normalizedHorizontalSpeed = 0;
        }

        if (_controller.isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

        }

        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        _velocity.y += gravity * Time.deltaTime;

        if (_controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;
    }
}
