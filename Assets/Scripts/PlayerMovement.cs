using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour, IDamage, IHeal, IPickup, IShieldPickups
{
    [SerializeField] LayerMask ignoreLayer;

    // Speed
    [SerializeField] CharacterController controller;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    // Jump
    [SerializeField] int jumpSpeed;
    [SerializeField] int JumpMax;
    [SerializeField] int gravity;

    //shoot
    [SerializeField] int shootDmg;
    [SerializeField] float shootRte;
    [SerializeField] int shootDist;

    //Health
    [SerializeField] int Hp;
    [SerializeField] shieldStats shield;
    [SerializeField] int shieldDurability;
    [SerializeField] bool shieldCharged;
    

    Vector3 moveDir;
    Vector3 playerVel;

    float shootTimer;

    int jumpCount;
    int HpOriginal;
    int CurrentHp;

    // Used for later on
    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HpOriginal = Hp;

        UpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.green);

        movement();
        sprint();
        ActivateShield();
    }

    void movement()
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

        jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= shootRte)
        {
            shoot();
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < JumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(dmg != null)
            {
                dmg.takeDamage(shootDmg);
            }
        }
    }

   

    public void UpdatePlayerUI() 
    {
        gameManager.instance.playerHPBar.fillAmount = (float)Hp / HpOriginal;
        gameManager.instance.shieldCharge.fillAmount = shieldCharged ? 1 : 0;
    }

    IEnumerator flashDmgScreen()
    {
        gameManager.instance.playerDmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDmgScreen.SetActive(false);
    }

    IEnumerator flashHealScreen()
    {
        gameManager.instance.playerHealScreen.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        gameManager.instance.playerHealScreen.SetActive(false);
    }

    public void takeDamage(int amount)
    {
         
        Hp -= amount;

        UpdatePlayerUI();
        StartCoroutine(flashDmgScreen());

        if (Hp <= 0)
        {
            gameManager.instance.youLose();
        }
    
}

    public void heal(int amount)
    {
        Hp += amount;

        UpdatePlayerUI();
        StartCoroutine(flashHealScreen());
        StopHeal(HpOriginal);

    }

    public void StopHeal(int HpOriginal)
    {
       if (CurrentHp <= HpOriginal)
        {
            CurrentHp = Hp;
        }
    }

    public void GetHealthStats(HealthPackStats health)
    {
        Hp += health.healthAmount;

        UpdatePlayerUI();
    }

    public void GetShieldStats(shieldStats newShield)
    {
        shield = newShield;
        shieldDurability = newShield.shieldDurability;
        shieldCharged = newShield.shieldCharged;

        UpdatePlayerUI();
    }

    void ActivateShield()
    {
        GameObject shield = GameObject.Find("Shield");
        CapsuleCollider shieldCollider = shield.GetComponent<CapsuleCollider>();

        if (Input.GetButtonDown("Shield") && shieldCharged == true)
        {
            shieldCollider.enabled = true;
            shieldCharged = false;
            gameManager.instance.shieldBar.enabled = true;

            UpdatePlayerUI();
        }

        if(shieldDurability == 0)
        {
            shieldCollider.enabled = false;
            gameManager.instance.shieldBar.enabled = false;

            UpdatePlayerUI();
        }
    }
}
