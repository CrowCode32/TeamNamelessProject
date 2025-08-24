using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class shieldStats : ScriptableObject, IDamage
{
    public GameObject model;
    [SerializeField] public int shieldDurability;
    [SerializeField] public bool shieldCharged;
    int maxDurability;

    public void Start()
    {
        shieldDurability = maxDurability;
        gameManager.instance.shieldCharge.fillAmount = shieldCharged ? 1 : 0;
    }

    public void takeDamage(int amount)
    {
        shieldDurability -= amount;
    }
}
