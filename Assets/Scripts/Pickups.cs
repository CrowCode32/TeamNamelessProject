using UnityEngine;

public class Pickups : MonoBehaviour
{
    
    [SerializeField] HealthPackStats health;
    [SerializeField] gunStats gun;


    public void OnTriggerEnter(Collider other)
    {
        IPickup pickupable = other.GetComponent<IPickup>();

        if (pickupable != null)
        {
            if (health != null) //Only if health pack exsist 
            {
                pickupable.GetHealthStats(health);
            }

            if (gun != null) //Only if gun exsist
            {
                pickupable.GetGunStats(gun);

            }
            Destroy(gameObject);
        }
    }

}
