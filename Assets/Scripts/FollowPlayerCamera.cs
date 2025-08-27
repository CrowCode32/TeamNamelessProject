using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour

{
    [SerializeField] private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
