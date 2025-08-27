using System.Collections;
using UnityEngine;

public class reloadPad : MonoBehaviour
{
    [SerializeField] Rigidbody Rb;

    [SerializeField] float reloadRate;
    [SerializeField] int ammoAmount;

    bool isReloading;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Reloading!");
        IReload ammo = other.GetComponent<IReload>();

        if (other.isTrigger)
            return;

        if (ammo !=null)
        {
            if(!isReloading)
            {
                StartCoroutine(ReloadAmmo(ammo));
            }
            
        }
    }

    IEnumerator ReloadAmmo(IReload ammo)
    {
        isReloading = true; 
        ammo.reloadAmmo(ammoAmount);
        yield return new WaitForSeconds(reloadRate);
        isReloading=false;
    }



}
