using UnityEngine;
using TeamNameless;
public class PlayerController : MonoBehaviour, IDamage,
{
    [Header("Health")]
    [SerializeField] private int HP = 100;

    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMod = 1.5f;

    [Header("Jumping")]
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private int jumpMax = 2;
    [SerializeField] private float gravity = 20f;

    [Header("Shooting")]
    [SerializeField] private float shootDist = 100f;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private int shootDamage = 10;

    private Vector3 moveDir;
    private Vector3 playerVel;
    private int jumpCount;
    private bool isSprinting;
    private float shootTimer;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
        Sprint();

        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = -1f; // Keeps grounded
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                  (Input.GetAxis("Vertical") * transform.forward);

        float currentSpeed = isSprinting ? speed * sprintMod : speed;
        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        Jump();

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            TeamNameless.IDamage dmg = hit.collider.GetComponent<TeamNameless.IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        Debug.Log($"Player took {amount} damage. Remaining HP: {HP}");

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

}
