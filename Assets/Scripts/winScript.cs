using UnityEngine;

public class winScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        
        if (other.CompareTag("Player"))
        {
           gameManager.instance.youWin();
        }
        
    }
}
