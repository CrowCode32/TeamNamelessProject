using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour
{
    enum HealType { Stationary, Pickup }
    [SerializeField] HealType type;

    [SerializeField] int healAmount;
    [SerializeField] int maxHealAmount;
    [SerializeField] int healTimer;

    bool isHealing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if(type == HealType.Stationary)
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerStay(Collider other)
    {
        IHeal heal = other.GetComponent<IHeal>();

        if (other.isTrigger)
            return;

        if (heal != null & type == HealType.Stationary)
        {
            if (!isHealing)
            {
                StartCoroutine(healOther(heal));
            }
        }
    }

    IEnumerator healOther(IHeal h)
    {
        isHealing = true;
        h.heal(healAmount);
        yield return new WaitForSeconds(healTimer);
        isHealing=false;
    }

    IEnumerator StopHeal(IHeal h)
    {
        isHealing = false;
        h.heal(maxHealAmount);
        yield return new WaitForSeconds(healTimer);
    }
}
