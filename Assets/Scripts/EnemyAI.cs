using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int Hp;
    [SerializeField] int FaceTargetSpeed;

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] GameObject Bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPOS;

    Color Originalcolor;

    float shootTimer;

    bool playerInTrigger;

    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Originalcolor = model.material.color;
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (playerInTrigger)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;

            agent.SetDestination(gameManager.instance.player.transform.position);

            if (shootTimer >= shootRate)
            {
                Shoot();
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * FaceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
    void Shoot()
    {
        shootTimer = 0;
        Instantiate(Bullet, shootPOS.position, transform.rotation);
    }

    public void TakeDamage(int amount)
    {
        if (Hp > 0)
        {
            Hp -= amount;
            StartCoroutine(flashRed());
        }
        if (Hp <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Originalcolor;
    }
}
