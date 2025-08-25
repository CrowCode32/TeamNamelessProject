using UnityEngine;

public class shieldPickup : MonoBehaviour
{
    [SerializeField] GameObject shieldUpgradePickup;
    [SerializeField] shieldUpgrade shieldStats;
    public static shieldPickup shieldPick;

    public void OnTriggerEnter(Collider other)
    {
        gameManager.instance.hasShield = true;
        shieldStats.shieldCharged = true;
        shieldStats.shieldDurability = shieldStats.maxDurability;
        Destroy(gameObject);
    }
}
