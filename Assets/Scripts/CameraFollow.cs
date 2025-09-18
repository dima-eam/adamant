using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player == null) return;
        transform.position = player.position + offset;
        // Optional: keep looking at the player
        transform.LookAt(player.position);
    }

}