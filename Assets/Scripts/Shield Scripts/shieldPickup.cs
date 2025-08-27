using UnityEngine;

public class shieldPickup : MonoBehaviour
{
    [SerializeField] shieldUpgrade playerShield;

    public void OnTriggerEnter(Collider other)
    {
        playerShield.shieldCharged = true;
        gameManager.instance.shieldCharge.fillAmount = playerShield.shieldCharged ? 1 : 0;
        playerShield.shieldDurability = playerShield.maxDurability;
        Destroy(gameObject);
    }
}
