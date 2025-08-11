using UnityEngine;

public class EmissiveFlicker : MonoBehaviour
{
    [SerializeField] private Material emissiveMat;

    private void Start()
    {
        if (emissiveMat == null)
        {
            Debug.LogError("Emissive material not assigned!");
            return;
        }

        Color baseColor = emissiveMat.GetColor("_EmissionColor");
        // Flicker logic here...
    }
}
