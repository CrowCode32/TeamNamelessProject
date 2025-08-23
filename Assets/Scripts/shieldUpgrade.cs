using UnityEngine;

public class shieldUpgrade : MonoBehaviour
{
    [SerializeField] CapsuleCollider shield;
    [SerializeField] int shieldDurability;
    [SerializeField] bool shieldCharged;
    int maxDurability;

    // Update is called once per frame
    private void Start()
    {
        maxDurability = shieldDurability;
    }

    void Update()
    {
        //If the player hasn't picked up the upgrade, return
        //If the player presses a certain button and the shield has charge, toggle on; else off
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            shieldDurability--;
        }
    }
}
