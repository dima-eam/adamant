using UnityEngine;

public class CollisionDectector : MonoBehaviour
{

    private Transform actor;

    void Start()
    {
        actor = GetComponent<Transform>();
        Debug.Log($"Initialized for {name}");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}, stop moving");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} triggered with {other.gameObject.name}, stop moving");
    }

}
