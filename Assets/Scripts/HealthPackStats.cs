using UnityEngine;

[CreateAssetMenu]

public class HealthPackStats : ScriptableObject
{
    public GameObject model;

    [Range(1, 10)] public int healthAmount;
}
