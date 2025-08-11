using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamage
{

    [SerializeField] int HP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int amount)
    {
        HP -= amount;
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

