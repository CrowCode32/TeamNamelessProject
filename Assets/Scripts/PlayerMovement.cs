using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamage, IHeal, IPickups, IReload
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

    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    //shoot
    [SerializeField] int shootDmg;
    [SerializeField] float shootRte;
    [SerializeField] int shootDist;

    //Health
    [SerializeField] int Hp;

    [SerializeField] GameObject shield;


    Vector3 moveDir;
    Vector3 playerVel;

    float shootTimer;

    int jumpCount;
    int HpOriginal;
    int CurrentHp;
    int gunListPos;

    // Used for later on
    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HpOriginal = Hp;
        gameManager.instance.playerAmmoBar.fillAmount = 0;
       spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.green);

        movement();
        sprint();

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

        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRte)

            shoot();
        selectGun();
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

    //void reload()
    //{
    //    if (Input.GetButtonDown("Reload"))
    //    {
    //        gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
    //    }
    //}

    void shoot()
    {
        shootTimer = 0;
        gunList[gunListPos].ammoCur--;
        UpdatePlayerAmmoBar();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);


            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDmg);
            }
        }
    }

     public void reloadAmmo(int amount)
    {
       
        if (gunList[gunListPos].ammoCur < gunList[gunListPos].ammoMax)
        {
            Debug.Log(amount);
            gunList[gunListPos].ammoCur += amount;
            UpdatePlayerAmmoBar();
        }
          //Need to make an ammo bar and then make sure this method differentiates/make new method.
    } 

    public void UpdatePlayerHPBar()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)Hp / HpOriginal;
    }

    public void UpdatePlayerAmmoBar()
    {
        gameManager.instance.playerAmmoBar.fillAmount = (float)gunList[gunListPos].ammoCur/gunList[gunListPos].ammoMax;
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

        UpdatePlayerHPBar();
        StartCoroutine(flashDmgScreen());

        if (Hp <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void heal(int amount)
    {
        if(Hp < HpOriginal)
        {
            Hp += amount;
            UpdatePlayerHPBar();
            StartCoroutine(flashHealScreen());
        }
        //StopHeal(HpOriginal);
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

        UpdatePlayerHPBar();
    }

    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;
        gameManager.instance.playerAmmoBar.fillAmount = 1;
        changeGun();
        
    }

    void changeGun()
    {
        shootDmg = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDistance;
        shootRte = gunList[gunListPos].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
        


    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
        {
            gunListPos++;
            changeGun();
            UpdatePlayerAmmoBar();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
            UpdatePlayerAmmoBar();
        }
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;

        playerVel = Vector3.zero;
        Hp = HpOriginal;
        UpdatePlayerHPBar();
    }
}
