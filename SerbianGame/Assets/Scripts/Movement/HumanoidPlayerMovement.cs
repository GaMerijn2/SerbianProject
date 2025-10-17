using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HumanoidPlayerMovement : MonoBehaviour, IHumanoidMoveable
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float crouchMultiplier;

    [Header("Component References")]
    [SerializeField] private CharacterController controller;

    public float MappedWalkSpeed
    {
        get => Map(0, 6, 0, 1, currentSpeed);
    }

    private Vector3 velocity;
    private bool isSprinting;
    private float currentSpeed;

    private void Awake()
    {
        SetMoveSpeed(walkSpeed);
    }

    private void Update()
    {
        velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    public void Move(Vector2 input)
    {
        if (!Camera.main)
            return;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * input.x + camForward * input.y;

        if (input != Vector2.zero && !isSprinting)
            SetMoveSpeed(walkSpeed);

        move *= currentSpeed;

        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += Physics.gravity.y * Time.deltaTime;

        Vector3 finalMove = move + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }


    public void StopMoving()
    {
        velocity = Vector3.zero;
    }

    public void SetMoveSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void RotateTowards(Vector2 input)
    {
        if (input == Vector2.zero)
            return;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    public void Jump()
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
    }

    public void Crouch(bool isCrouching)
    {
    }

    public void Dash()
    {
    }

    public void Roll()
    {
    }

    public void Sprint(bool isSprinting)
    {
        Debug.Log("Sprinting: " + isSprinting);
        this.isSprinting = isSprinting;
        float newSpeed = walkSpeed * (isSprinting ? sprintMultiplier : 1f);
        SetMoveSpeed(newSpeed);
    }

    public void WalkSlow(bool isSlowWalking)
    {
        float newSpeed = isSlowWalking ? walkSpeed * crouchMultiplier : walkSpeed;
        SetMoveSpeed(newSpeed);
    }

    private float Map(
        float inMin,
        float inMax,
        float outMin,
        float outMax,
        float value)
    {
        return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }

}
