using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour, IDamage, IHeal, IPickup
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
    [SerializeField] List<GunStats> gunList = new List<GunStats>();

    //shoot
    [SerializeField] int shootDmg;
    [SerializeField] float shootRte;
    [SerializeField] int shootDist;

    //Health
    [SerializeField] int Hp;

    Vector3 moveDir;
    Vector3 playerVel;

    float shootTimer;

    int jumpCount;
    int HpOriginal;
    int CurrentHp;
    int gunListPosition;

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

        if(!gameManager.instance.isPaused)
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

        if (Input.GetButton("Fire1") && shootTimer > shootRte && gunList.Count > 0 && gunList[gunListPosition].ammoCurr > 0)
        {
            shoot();
        }

        changeGun();

        reload();
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
        gunList[gunListPosition].ammoCurr--;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            Instantiate(gunList[gunListPosition].hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(dmg != null)
            {
                dmg.takeDamage(shootDmg);
            }
        }
    }

    void reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            gunList[gunListPosition].ammoCurr = gunList[gunListPosition].ammoMax;
        }
    }

   

    public void UpdatePlayerUI() 
    {
        gameManager.instance.playerHPBar.fillAmount = (float)Hp / HpOriginal;
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

    public void getGunStats(GunStats gun)
    {
        gunList.Add(gun);
        gunListPosition = gunList.Count -1;

        switchGun();
    }

    void switchGun()
    {
        shootDmg = gunList[gunListPosition].shootDamage;
        shootDist = gunList[gunListPosition].shootDistance;
        shootRte = gunList[gunListPosition].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPosition].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPosition].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void changeGun()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPosition < gunList.Count - 1)
        {
            gunListPosition++;
            switchGun();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPosition > 0)
        {
            gunListPosition--;
            switchGun();
        }
    }
}
