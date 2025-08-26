using UnityEngine;

public interface IHeal
{
    void heal(int amount);
    void StopHeal(); // No parameter version to match PlayerMovement
}
