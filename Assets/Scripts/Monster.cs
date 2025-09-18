using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Monster : MonoBehaviour
{
    public float maxHealth = 100f;
    public float deathForce = 30f; 

    private float currentHealth;
    private Rigidbody rb;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        // Monster is kinematic while alive
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("PlayerAttack"))
        {
            var damage = other.gameObject.GetComponent<WeaponBase>().damage;
            // Calculate hit direction from weapon to monster
            Vector3 hitDirection = (transform.position - other.transform.position).normalized;
            TakeDamage(damage, hitDirection); // TODO invert the logic, weapon collision triggers monster to lose health

            Debug.Log($"{name} got hit by {damage} damage");
        }
    }

    void TakeDamage(float amount, Vector3 hitDirection)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die(hitDirection);
        }
    }

    void Die(Vector3 hitDirection)
    {
        isDead = true;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce((hitDirection + Vector3.up) * deathForce, ForceMode.Impulse);

        Destroy(gameObject, 15f);
    }

}