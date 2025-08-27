using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] Transform destination;
    [SerializeField] Transform platform;

    Vector3 startingPos;

    void Start()
    {
        startingPos = platform.position;
    }

    // Update is called once per frame
    void Update()
    {
        platform.transform.position = Vector3.MoveTowards(platform.position, destination.position, speed * Time.deltaTime);
    }
}
