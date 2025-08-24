using UnityEngine;

public class shieldPickups : MonoBehaviour
{
    [SerializeField] shieldStats shieldStats;


    public void OnTriggerEnter(Collider other)
    {
        IShieldPickups pickupable = other.GetComponent<IShieldPickups>();

        if (pickupable != null)
        {
            pickupable.GetShieldStats(shieldStats);

            Destroy(gameObject);
        }
    }
}
