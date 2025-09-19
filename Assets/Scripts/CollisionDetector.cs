using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private Transform actor;

    private void Start()
    {
        actor = GetComponent<Transform>();
        Debug.Log($"Initialized for {name}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}, stop moving");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} triggered with {other.gameObject.name}, stop moving");
    }
}