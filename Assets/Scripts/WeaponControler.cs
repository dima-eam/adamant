using UnityEngine;

// Defines common attacking logic for different weapons, and serves as a connecting point between the weapon and player.
public class WeaponController : MonoBehaviour
{

    public GameObject weaponPrefab;

    private WeaponBase weapon;
    private bool isAttacking; // TODO think about animation cancellation

    void Start()
    {
        EquipWeapon(weaponPrefab);
        Debug.Log($"Weapon equipped: {weaponPrefab.name}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            StartCoroutine(AttackLoop());
        }
        if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
        }
    }

    private System.Collections.IEnumerator AttackLoop()
    {
        while (isAttacking && weapon != null)
        {
            yield return StartCoroutine(weapon.AttackRoutine());
        }
    }

    private void EquipWeapon(GameObject weaponPrefab)
    {
        var weaponInstance = Instantiate(weaponPrefab, transform);
        weaponInstance.transform.SetLocalPositionAndRotation(Vector3.forward, Quaternion.identity);

        weapon = weaponInstance.GetComponent<WeaponBase>();
    }

}