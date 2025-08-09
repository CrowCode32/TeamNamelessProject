using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum DamageType { moving, stationary, DOT, Homing }
    [SerializeField] DamageType type;
    [SerializeField] Rigidbody Rb;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int MoveSpeed;
    [SerializeField] int destroyTime;

    bool isDamaging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == DamageType.moving || type == DamageType.Homing)
        {
            Destroy(gameObject, destroyTime);

            if (type == DamageType.moving)
            {
                Rb.linearVelocity = transform.forward * MoveSpeed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == DamageType.Homing)
        {
            //Rb.linearVelocity = (gameManager.instance.player.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (other.isTrigger)
            return;

        if (dmg != null && type != DamageType.DOT)
        {
            dmg.TakeDamage(damageAmount);
        }

        if (type == DamageType.moving || type == DamageType.Homing)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (other.isTrigger)
            return;

        if (dmg != null && type == DamageType.DOT)
        {
            if (!isDamaging)
            {
                StartCoroutine(damageOther(dmg));
            }
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.TakeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }

}

