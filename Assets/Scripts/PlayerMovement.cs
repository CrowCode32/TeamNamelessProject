using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamage // IHeal
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
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    //Health
    [SerializeField] int Hp;


    // Gun
    [SerializeField] GameObject gunModel;
    //[SerializeField] List<gunStats> gunList = new List<gunStats>();

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
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.green);

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

        //if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRate)
            shoot();

        //selectGun();
        //reload();
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

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
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
        gameManager.instance.playerHPBar.fillAmount = (float)Hp / HpOriginal;
    }

    IEnumerator flashDmgScreen()
    {
        gameManager.instance.playerDmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDmgScreen.SetActive(false);
    }

    //IEnumerator flashHealScreen()
    //{
    //    gameManager.instance.playerHealScreen.SetActive(true);
    //    yield return new WaitForSeconds(0.2f);
    //    gameManager.instance.playerHealScreen.SetActive(false);
    //}

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

    //void changeGun()
    //{
    //    shootDmg = gunList[gunListPos].shootDamage;
    //    shootDistance = gunList[gunListPos].shootDistance;
    //    shootRate = gunList[gunListPos].shootRate;

    //    gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
    //    gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
    //}

    //void selectGun()
    //{
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
    //    {
    //        gunListPos++;
    //        changeGun();
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
    //    {
    //        gunListPos--;
    //        changeGun();
    //    }
    //}

    //public void heal(int amount)
    //{
    //    Hp += amount;

    //    UpdatePlayerUI();
    //    StartCoroutine(flashHealScreen());
    //    StopHeal(HpOriginal);

    //}

    //public void StopHeal(int HpOri
    //{
    //    if (CurrentHp <= HpOriginal)
    //    {
    //        CurrentHp = Hp;
    //    }
    //}
}

    // Started making a heal not realizing someone else had already started
//    // Heal
//    public void Heal(int amount)
//    {
//        Hp = Mathf.Max(Hp + amount, HpOriginal);
//        UpdatePlayerUI();
//    }