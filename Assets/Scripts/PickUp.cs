using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();

        if(pick != null)
        {
            pick.getGunStats(gun);
            gun.ammoCur = gun.ammoMax;
            Destroy(gameObject);
        }
    }
}
