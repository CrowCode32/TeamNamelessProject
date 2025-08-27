using UnityEngine;

public class shieldUpgrade : MonoBehaviour, IDamage
{
    [SerializeField] CapsuleCollider shield;
    [SerializeField] public int shieldDurability;
    public int maxDurability;
    public bool shieldCharged;

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
            Debug.Log("Shield bar on");
            gameManager.instance.shieldBar.enabled = true;
            gameManager.instance.shieldCharge.fillAmount = shieldCharged ? 1 : 0;
        } 

        if(shieldDurability <= 0)
        {
            shield.enabled = false;
            Debug.Log("Shield bar off");
            gameManager.instance.shieldBar.enabled = false;
        }
        
    }

    public void takeDamage(int amount)
    {
        shieldDurability -= amount;
    }
}
