using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour, IDamage
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

    Vector3 moveDir;
    Vector3 playerVel;

    float shootTimer;

    int jumpCount;
    int HpOriginal;

    // Used for later on
    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HpOriginal = Hp;

     /*   UpdatePlayerUI();*/
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
        }
    }

    public void TakeDamage(int amount)
    {
        Hp -= amount;

      /*  UpdatePlayerUI();*/
        StartCoroutine(flashDmgScreen());

        if(Hp <= 0)
        {
            gameManager.instance.youLose();
        }
    }

  /*  public void UpdatePlayerUI() 
    {
        gameManager.instance.playerHPBar.fillAmount = (float)Hp / HpOriginal;
    }*/

    IEnumerator flashDmgScreen()
    {
        gameManager.instance.playerDmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDmgScreen.SetActive(false);
    }
}
