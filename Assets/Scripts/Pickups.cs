using UnityEngine;

public class Pickups : MonoBehaviour
{
    
    [SerializeField] HealthPackStats health;


    public void OnTriggerEnter(Collider other)
    {
        IPickup pickupable = other.GetComponent<IPickup>();

        if(pickupable != null )
        {
            pickupable.GetHealthStats(health);
            
            Destroy(gameObject);
        }
    }
}
