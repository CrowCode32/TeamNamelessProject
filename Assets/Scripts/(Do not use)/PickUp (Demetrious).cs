using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] GunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();

        if(pick != null)
        {
            pick.getGunStats(gun);
            gun.ammoCurrent = gun.ammoMax;
            Destroy(gameObject);
        }
    }
}
