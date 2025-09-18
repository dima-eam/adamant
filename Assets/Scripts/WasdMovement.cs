using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    private static readonly int Direction = Animator.StringToHash("moveDirection");

    public float moveSpeed = 5f;

    private Animator animator;
    private CharacterController controller;

    private MoveDirection direction = MoveDirection.Idle;
    private int weaponType = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            weaponType = 1;
            animator.SetInteger("weaponType", weaponType);
            Debug.Log($"Switching to combat: d={animator.GetInteger("moveDirection")}, w={animator.GetInteger("weaponType")}");
        }
        
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input2 = new Vector2(h, v);
        direction = input2 == Vector2.zero ? MoveDirection.Idle : MoveDirection.RunForward;
        animator.SetInteger(Direction, (int)direction);
        if (direction == MoveDirection.Idle)
        {
            return;
        }

        // Input in local camera space
        var input3 = new Vector3(h, 0f, v).normalized;
        controller.Move(input3 * (Time.deltaTime * moveSpeed));

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