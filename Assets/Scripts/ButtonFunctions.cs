using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonFunctions : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject settingsMenu;

    public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void respawn()
    {
        //gameManager.instance.player.transform.position = gameManager.instance.;
        //gameManager.instance.player.transform.rotation = gameManager.instance.startRot;
        //gameManager.instance.stateUnpause();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

    public void settings()
    {
        menuPause.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void backToPause()
    {
        settingsMenu.SetActive(false);
        menuPause.SetActive(true);
    }
}
