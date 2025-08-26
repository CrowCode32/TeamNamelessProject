using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour, IDamage, IHeal, IPickup
{
    [SerializeField] LayerMask ignoreLayer;

    // Movement
    [SerializeField] CharacterController controller;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    // Jump
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    // Shooting
    [SerializeField] int shootDmg;
    [SerializeField] float shootRte;
    [SerializeField] int shootDist;

    // Health
    [SerializeField] int hp;
    [SerializeField] GameObject shield;

    Vector3 moveDir;
    Vector3 playerVel;
    float shootTimer;
    int jumpCount;
    int hpOriginal;
    int currentHp;
    bool isSprinting;

    void Start()
    {
        hpOriginal = hp;
        currentHp = hp;
        UpdatePlayerUI();
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.green);
        HandleMovement();
        HandleSprint();
    }

    void HandleMovement()
    {
        shootTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                  (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);
        HandleJump();
        controller.Move(playerVel * Time.deltaTime);

        if (Input.GetButton("Fire1") && shootTimer >= shootRte)
        {
            HandleShoot();
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void HandleSprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;         // Increase speed
            isSprinting = true;         // Flag sprinting
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;         // Revert speed
            isSprinting = false;        // Clear sprint flag
        }
    }

    void HandleShoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDmg);
            }
        }
    }

    public void UpdatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)hp / hpOriginal;
    }

    IEnumerator FlashDamageScreen()
    {
        gameManager.instance.playerDmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDmgScreen.SetActive(false);
    }

    IEnumerator FlashHealScreen()
    {
        gameManager.instance.playerHealScreen.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        gameManager.instance.playerHealScreen.SetActive(false);
    }

    public void takeDamage(int amount)
    {
        hp -= amount;
        UpdatePlayerUI();
        StartCoroutine(FlashDamageScreen());

        if (hp <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void heal(int amount)
    {
        hp += amount;
        UpdatePlayerUI();
        StartCoroutine(FlashHealScreen());
        StopHeal();
    }

    public void StopHeal()
    {
        if (hp > hpOriginal)
        {
            hp = hpOriginal;
            UpdatePlayerUI();
        }
    }

    public void GetHealthStats(HealthPackStats health)
    {
        hp += health.healthAmount;
        UpdatePlayerUI();
        StopHeal();
    }

    // ✅ IPickup interface implementation
    public void getGunStats(gunStats gunStats)
    {
        shootDmg = gunStats.damage;
        shootRte = gunStats.fireRate;
        shootDist = gunStats.range;

        Debug.Log($"Gun stats applied: Damage={shootDmg}, Rate={shootRte}, Range={shootDist}");
    }
}
