using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [SerializeField] TMP_Text gameGoalCountText;

    public Image playerHPBar;
<<<<<<< HEAD
    public GameObject playerDmgScreen;
=======
    public GameObject playerDamageScreen;
>>>>>>> aeaf91e4e61bb1040bf695f869a02bff3901d867

    public GameObject player;
    public PlayerMovement playerScript;

<<<<<<< HEAD
=======
    public Vector3 startPos;
    public Quaternion startRot;

>>>>>>> aeaf91e4e61bb1040bf695f869a02bff3901d867
    public bool isPaused;

    float timeScaleOrig;

    int gameGoalCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
<<<<<<< HEAD
=======

        startPos = player.transform.position;
        startRot = player.transform.rotation;
>>>>>>> aeaf91e4e61bb1040bf695f869a02bff3901d867
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

<<<<<<< HEAD
    
=======
>>>>>>> aeaf91e4e61bb1040bf695f869a02bff3901d867
    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;

<<<<<<< HEAD
        gameGoalCountText.text = gameGoalCount.ToString("F0");    

        if(gameGoalCount <= 0)
=======
        gameGoalCountText.text = gameGoalCount.ToString("F0");
        if (gameGoalCount <= 0)
>>>>>>> aeaf91e4e61bb1040bf695f869a02bff3901d867
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
<<<<<<< HEAD
       
    }

    
=======
    }

>>>>>>> aeaf91e4e61bb1040bf695f869a02bff3901d867
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}
