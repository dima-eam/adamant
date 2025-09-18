using System;
using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator animator;
    private CharacterController controller;
    private Camera mainCamera;

    private MoveDirection direction = MoveDirection.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input2 = new Vector2(h, v);
        direction = input2 switch
        {
            { x: 0, y: 0 } => MoveDirection.Idle,
            { x: 0, y: 1 } => MoveDirection.RunForward,
            // { x: 0, y: -1 } => MoveDirection.RunBackward,
            // { x: -1, y: 0 } => MoveDirection.RunLeft,
            // { x: 1, y: 0 } => MoveDirection.RunRight,
            // { x: -1, y: 1 } => MoveDirection.RunForwardLeft,
            // { x: 1, y: 1 } => MoveDirection.RunForwardRight,
            // { x: -1, y: -1 } => MoveDirection.RunBackwardLeft,
            // { x: 1, y: -1 } => MoveDirection.RunBackwardRight,
            { x: 0, y: -1 } => MoveDirection.RunForward,
            { x: -1, y: 0 } => MoveDirection.RunForward,
            { x: 1, y: 0 } => MoveDirection.RunForward,
            { x: -1, y: 1 } => MoveDirection.RunForward,
            { x: 1, y: 1 } => MoveDirection.RunForward,
            { x: -1, y: -1 } => MoveDirection.RunForward,
            { x: 1, y: -1 } => MoveDirection.RunForward,
            _ => MoveDirection.Idle
        };
        // Debug.Log($"Drection={direction}");

        animator.SetInteger("moveDirection", (int)direction);

        // Input in local camera space
        var input3 = new Vector3(h, 0f, v).normalized;
        controller.Move(input3 * Time.deltaTime * moveSpeed);

        var rotation = transform.rotation;
        var targetRotation = Quaternion.LookRotation(input3);
        transform.rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * 10f);

        // TODO add dash for space bar
    }

    // Mapped to PlayerAnimation controller parameters and transitions
    private enum MoveDirection
    {
        Idle = 0,
        RunForward = 1, // W
        RunLeft = 2, // A
        RunBackward = 3, // S
        RunRight = 4, // D
        RunForwardLeft = 5,
        RunForwardRight = 6,
        RunBackwardLeft = 7,
        RunBackwardRight = 8
    }

}