using UnityEngine;

public class Pickups : MonoBehaviour
{
    
    //[SerializeField] HealthPackStats health;

    [SerializeField] gunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickups pickupable = other.GetComponent<IPickups>();

        if (pickupable != null)
        {
            pickupable.getGunStats(gun);
            gun.ammoCur = gun.ammoMax;
            Destroy(gameObject);
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    IPickup pickupable = other.GetComponent<IPickup>();

    //    if (pickupable != null)
    //    {
    //        pickupable.GetHealthStats(health);

    //        Destroy(gameObject);
    //    }
    //}
}
