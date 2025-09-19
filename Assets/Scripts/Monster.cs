using System;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
public class Monster : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float attackDistance = 2f;
    public float moveSpeed = 3f;
    public float maxHealth = 100f;

    private float currentHealth;

    private Transform target;
    private Animator animator;

    private bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        Debug.Log($"{name} initialized: health={currentHealth}");
    }

    private void Update()
    {
        // Find player if no target yet
        if (!target)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player && InDetectionRange(player.transform))
            {
                target = player.transform;
            }
        }

        // Player is not in detection radius
        if (!target) return;

        var distance = DistanceXZ(target.position);
        if (distance <= attackDistance)
        {
            animator.SetInteger("action", 2);
        }
        else if (distance <= detectionRadius)
        {
            // Chase
            animator.SetInteger("action", 1);
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0; // ignore vertical movement
            transform.position += direction * (moveSpeed * Time.deltaTime);

            // Face the player
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
            }
        }
    }

    private float DistanceXZ(Vector3 targetPos)
    {
        Vector2 a = new Vector2(transform.position.x, transform.position.z);
        Vector2 b = new Vector2(targetPos.x, targetPos.z);
        return Vector2.Distance(a, b);
    }

    private bool InDetectionRange(Transform t)
    {
        return DistanceXZ(t.position) <= detectionRadius;
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

            Debug.Log($"{name} got hit by {damage} damage trigger");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isDead) return;

        if (other.gameObject.CompareTag("PlayerAttack"))
        {
            var damage = other.gameObject.GetComponent<WeaponBase>().damage;
            // Calculate hit direction from weapon to monster
            Vector3 hitDirection = (transform.position - other.transform.position).normalized;
            TakeDamage(damage, hitDirection); // TODO invert the logic, weapon collision triggers monster to lose health

            Debug.Log($"{name} got hit by {damage} damage collision");
        }
    }

    private void TakeDamage(float amount, Vector3 hitDirection)
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

        // rb.isKinematic = false;
        // rb.useGravity = true;
        // rb.AddForce((hitDirection + Vector3.up) * deathForce, ForceMode.Impulse);

        Destroy(gameObject, 15f);
    }
}