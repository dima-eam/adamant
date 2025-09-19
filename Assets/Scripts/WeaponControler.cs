using UnityEngine;

// Defines common attacking logic for different weapons, and serves as a connecting point between the weapon and player.
public class WeaponController : MonoBehaviour
{
    public GameObject weaponPrefab;

    private Animator animator;
    private Camera mainCamera;
    private WeaponBase weapon;
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        // EquipWeapon();
    }

    void Update()
    {
        if (isAttacking)
        {
            RotateToMouse();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            animator.SetBool("attack", isAttacking);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
            animator.SetBool("attack", isAttacking);
        }
    }

    private void RotateToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane ground = new Plane(Vector3.up, transform.position);
        if (ground.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Vector3 dir = hit - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, lookRot.eulerAngles.y, 0f);
            }
        }
    }

    private void EquipWeapon()
    {
        var weaponInstance = Instantiate(weaponPrefab, transform);
        weaponInstance.transform.SetLocalPositionAndRotation(Vector3.forward, Quaternion.identity);

        weapon = weaponInstance.GetComponent<WeaponBase>();
        Debug.Log($"Weapon equipped: {weaponPrefab.name}");
    }
}