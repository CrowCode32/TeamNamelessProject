using UnityEngine;

public class doorCondition : MonoBehaviour
{
    [SerializeField] GameObject doorPrefab;
    [SerializeField] GameObject bossEnemy;
    [SerializeField] Vector3 openOffset = new Vector3(-10f, 0f, 0f);

    Vector3 openPos;
    void Start()
    {
        openPos = doorPrefab.transform.position + openOffset;
    }

    
    void Update()
    {
        if (bossEnemy == null)
        {
            doorPrefab.transform.position = Vector3.Lerp(doorPrefab.transform.position, openPos, Time.deltaTime * 2);
        }
    }
}
