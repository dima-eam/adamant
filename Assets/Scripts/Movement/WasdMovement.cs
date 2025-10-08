using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    private static readonly int Direction = Animator.StringToHash("moveDirection");

    public float moveSpeed = 5f;
    public GameObject swordPrefab;

    private Animator animator;
    private CharacterController controller;
    private Transform rightHand;
    private Transform mainCamera;
    private GameObject currentSword;
    private Interactable currentTarget;
    private bool autoMoving;

    private MoveDirection direction = MoveDirection.Idle;
    private int weaponType = -1; // Non combat

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("weaponType", weaponType);

        controller = GetComponent<CharacterController>();

        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand).Find("B-handProp.R");

        mainCamera = Camera.main.transform;
    }

    private void Update()
    {
        MaybeEquipWeapon();
        MaybeWasdMove();
        MaybeInteract();
        MaybeAutoMove();
    }

    private void MaybeInteract()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        var ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;
            
        var interactable = hit.collider.GetComponent<Interactable>();
        if (!interactable) return;
                
        currentTarget = interactable;
        autoMoving = true;
    }

    private void MaybeAutoMove()
    {
        if (!autoMoving || !currentTarget) return;
        
        var targetPos = currentTarget.transform.position;
        var to = (targetPos - transform.position);
        to.y = 0f;
        var distance = to.magnitude;
        if (distance > 2)
        {
            // Move toward target
            Vector3 step = to.normalized * moveSpeed * Time.deltaTime;
            Debug.Log($"Auto moving: step={step}");
            controller.Move(step * (Time.deltaTime * moveSpeed));

            var rotation = transform.rotation;
            var targetRotation = Quaternion.LookRotation(step);
            transform.rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            Debug.Log("Interacting");
            // Within range — interact once, then stop
            currentTarget.OnInteract(this);
            autoMoving = false;
        }
    }

    private void MaybeEquipWeapon()
    {
        if (!Input.GetKeyDown(KeyCode.Alpha1)) return;
        
        weaponType = weaponType > -1 ? -1 : 1;
        animator.SetInteger("weaponType", weaponType);
        Debug.Log($"Switching to combat: d={animator.GetInteger("moveDirection")}, w={animator.GetInteger("weaponType")}");
        currentSword = Instantiate(swordPrefab, rightHand);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;
    }

    private void MaybeWasdMove()
    {
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

        autoMoving = false;

        // Input in local camera space
        var forward = mainCamera.forward.normalized;
        var right = mainCamera.right.normalized;
        // Ignore vertical tilt (so movement is flat)
        forward.y = 0f;
        right.y = 0f;
        var input3 = (forward * v + right * h).normalized;
        controller.SimpleMove(input3 * (Time.deltaTime * moveSpeed));

        var rotation = transform.rotation;
        var targetRotation = Quaternion.LookRotation(input3);
        transform.rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * 10f);

        // TODO add dash for space bar
    }

    // Mapped to PlayerAnimation controller parameters and transitions
    private enum MoveDirection
    {
        Idle = 0,
        RunForward = 1,
        RunBackward = 2
    }
}