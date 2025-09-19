using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    private static readonly int Direction = Animator.StringToHash("moveDirection");

    public float moveSpeed = 5f;
    public GameObject swordPrefab;

    private Animator animator;
    private CharacterController controller;
    private Transform rightHand;
    private GameObject currentSword;

    private MoveDirection direction = MoveDirection.Idle;
    private int weaponType = -1; // Non combat

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("weaponType", weaponType);

        controller = GetComponent<CharacterController>();

        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand).Find("B-handProp.R");
        Debug.Log($"Right hand: {rightHand}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = weaponType > -1 ? -1 : 1;
            animator.SetInteger("weaponType", weaponType);
            Debug.Log(
                $"Switching to combat: d={animator.GetInteger("moveDirection")}, w={animator.GetInteger("weaponType")}");
            currentSword = Instantiate(swordPrefab, rightHand);
            currentSword.transform.localPosition = Vector3.zero;
            currentSword.transform.localRotation = Quaternion.identity;
        }

        if (animator.GetBool("attack"))
        {
            return;
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