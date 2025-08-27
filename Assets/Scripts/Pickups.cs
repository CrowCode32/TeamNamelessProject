using UnityEngine;

public class Pickups : MonoBehaviour
{
    
    [SerializeField] HealthPackStats health;


    public void OnTriggerEnter(Collider other)
    {
        IPickups pickupable = other.GetComponent<IPickups>();

        if(pickupable != null )
        {
            pickupable.GetHealthStats(health);
            
            Destroy(gameObject);
        }
    }
}
