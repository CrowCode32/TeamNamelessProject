using UnityEngine;

public interface IHeal
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void heal(int amount);
    void StopHeal(int HpOriginal);
}
