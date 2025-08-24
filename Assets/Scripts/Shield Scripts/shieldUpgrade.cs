using UnityEngine;

public class shieldUpgrade : MonoBehaviour, IDamage
{
    [SerializeField] CapsuleCollider shield;
    [SerializeField] int shieldDurability;
    [SerializeField] bool shieldCharged;
    [SerializeField] float chargeAmount;
    [SerializeField] int shieldDmg;
    int maxDurability;

    // Update is called once per frame
    private void Start()
    {
        maxDurability = shieldDurability;
        gameManager.instance.shieldCharge.fillAmount = shieldCharged ? 1 : 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Shield") && shieldCharged == true)
        {
            shieldDurability = maxDurability;
            shield.enabled = true;
            shieldCharged = false;
            gameManager.instance.shieldBar.enabled = true;
        } 

        if(shieldDurability == 0)
        {
            shield.enabled = false;
            gameManager.instance.shieldBar.enabled = false;
        }
        
    }

    public void takeDamage(int amount)
    {
        shieldDurability -= amount;
    }
}
