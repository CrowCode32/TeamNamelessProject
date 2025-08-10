using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    Vector3 moveDir;
    Vector3 playerVel;

    float shootTimer;

    int jumpCount;

    // Used for later on
    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
}
