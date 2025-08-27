using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] Transform destination;
    [SerializeField] Transform platform;
    [SerializeField] Transform initialPosition;

    Vector3 startingPos;

    void Start()
    {
        startingPos = platform.position;
    }

    // Update is called once per frame
    void Update()
    {
        platform.transform.position = Vector3.MoveTowards(platform.position, destination.position, speed * Time.deltaTime);

        if(platform.position == destination.position)
        {
            destination.position = startingPos;
            startingPos = platform.position;
        }
    }
}
