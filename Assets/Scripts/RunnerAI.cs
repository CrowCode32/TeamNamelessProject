using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class RunnerAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
   // [SerializeField] Animator anim;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animSpeedTrans;

    [SerializeField] GameObject mine;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;



    Color colorOrig;

    float shootTimer;
    float angleToPlayer;
    float roamTimer;
    float stoppingDistanceOriginal;

    bool playerInTrigger;

    Vector3 playerDirection;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistanceOriginal = agent.stoppingDistance;

    }

    // Update is called once per frame
    void Update()
    {
        setAnimations();
        shootTimer += Time.deltaTime;

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

        if (playerInTrigger && !CanSeePlayer())
        {
            checkRoam();
        }
        else if (!playerInTrigger)
        {
            checkRoam();
        }
    }

    void setAnimations()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
      //  float animSpeedCurr = anim.GetFloat("Speed");

      //  anim.SetFloat("Speed", Mathf.Lerp(animSpeedCurr, agentSpeed, Time.deltaTime * animSpeedTrans));
    }

    void checkRoam()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 randomPos = Random.insideUnitSphere * roamDistance;
        randomPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
        agent.SetDestination(hit.position);
    }

    bool CanSeePlayer()
    {
        playerDirection = gameManager.instance.player.transform.position - transform.position;

        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(transform.position, playerDirection);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerDirection, out hit))
        {

            //Can See You!!
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {

                //agent.SetDestination(gameManager.instance.player.transform.position);
                Vector3 fleeDirection = (transform.position - gameManager.instance.player.transform.position).normalized;
                Vector3 fleeTarget = transform.position + fleeDirection * roamDistance;

                NavMeshHit fleeHit;

                if(NavMesh.SamplePosition(fleeTarget, out fleeHit, roamDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(fleeHit.position);
                }

                if (shootTimer >= shootRate)
                {
                    drop();
                }
                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
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
            agent.stoppingDistance = 0;
        }
    }
    void drop()
    {
        shootTimer = 0;

      //  anim.SetTrigger("Shoot");

        Vector3 dropPos = transform.position - transform.forward * 1.5f;
        Instantiate(mine, transform.position, Quaternion.identity);
    }

    public void takeDamage(int amount)
    {

        if (HP > 0)
        {
            HP -= amount;
            StartCoroutine(flashRed());

            agent.SetDestination(gameManager.instance.player.transform.position);
        }
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

}