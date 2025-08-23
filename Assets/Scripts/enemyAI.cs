using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] float bossAttackRate;
    [SerializeField] Transform shootPos;

    [SerializeField] bool IsBoss;
    [SerializeField] GameObject laserBeam;
    [SerializeField] GameObject shockwave;
    [SerializeField] GameObject mines;


    Color colorOrig;

    float shootTimer;
    float bossAttackTimer;

    bool playerInTrigger;

    Vector3 playerDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

        shootTimer += Time.deltaTime;
        bossAttackTimer += Time.deltaTime;

        if (playerInTrigger)
        {
            playerDirection = gameManager.instance.player.transform.position - transform.position;

            agent.SetDestination(gameManager.instance.player.transform.position);
            if (shootTimer >= shootRate)
            {
                shoot();

                if (IsBoss)
                {
                    if (bossAttackTimer > bossAttackRate)
                    {
                        bossAttacks();
                    }
                }
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
        }

      

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
        }
    }
    void shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {

        if (HP > 0)
        {
            HP -= amount;
            StartCoroutine(flashRed());
        }
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    IEnumerator flashRed()
    {
        Color tempColorOrig = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = tempColorOrig;
    }

    IEnumerator bossIndicator()
    {
        model.material.color = Color.purple;
        yield return new WaitForSeconds(3f);
        model.material.color = colorOrig;
    }

    IEnumerator bossIndicatorTwo()
    {
        model.material.color = Color.orange;
        yield return new WaitForSeconds(3f);
        model.material.color = colorOrig;
    }

    IEnumerator bossIndicatorThree()
    {
        model.material.color = Color.grey;
        yield return new WaitForSeconds(2f);
        model.material.color = colorOrig;
    }

    IEnumerator bossIndicatorFour()
    {
        model.material.color = Color.deepPink;
        yield return new WaitForSeconds(2f);
        model.material.color = colorOrig;
    }

    IEnumerator bossAttackOne()
    {
        
        agent.speed = agent.speed*3;
        shootRate = shootRate / 5;
        yield return new WaitForSeconds(3f);
        agent.speed = agent.speed / 3;
        shootRate = shootRate * 5;
    }

    IEnumerator bossAttackTwo()
    {
        //Shockwave Attack

        shockwave.transform.localScale = Vector3.zero;
        shockwave.gameObject.SetActive(true);
      

        float growTime = 2.5f; //Attack stats. Can be changed if needed.
        float maxScale = 10f;
        float elapsed = 0f;

        while (elapsed < growTime)  //while elapsed < growTime, expand.
        {
            elapsed += Time.deltaTime;
            float t = elapsed / growTime;

            shockwave.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxScale, t);

            yield return null;
        }

            yield return new WaitForSeconds(.5f);
            shockwave.gameObject.SetActive(false);
        

    }
    IEnumerator bossAttackThree()
    {
       //Throw Mines everywhere
        yield return new WaitForSeconds(2f);
        
    }
    IEnumerator bossAttackFour()
    {
        //Sweeping Laser Beam
        yield return new WaitForSeconds(1.5f); //Delay for the indicator
        laserBeam.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        laserBeam.gameObject.SetActive(false);

    }


    public void bossAttacks()
    {
       
            bossAttackTimer = 0;
            int attack = Random.Range(1,5);  


            if(attack == 1)
            {
                StartCoroutine(bossIndicator());
                StartCoroutine(bossAttackOne());
              
            }
            else if (attack == 2)
            {
                StartCoroutine(bossIndicatorTwo());
                StartCoroutine(bossAttackTwo());
            }
            else if (attack == 3)
            {
                StartCoroutine(bossIndicatorThree());
                StartCoroutine(bossAttackThree());
            }
            else if (attack == 4)
            {
                StartCoroutine(bossIndicatorFour());
                StartCoroutine(bossAttackFour());
            }
        
    }
}
