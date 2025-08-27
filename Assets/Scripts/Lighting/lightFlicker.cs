using UnityEngine;
using UnityEngine.Rendering;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight;
    public float flickerInterval = 0.1f;
    private float timer;

    //public float minIntesity = 0.4f;
    //public float maxIntesity = 2.0f;
    //public float flickerSpeed = 0.1f;

    void Update()
    {
        //Rapid flicker
        timer += Time.deltaTime;
        if (timer >= flickerInterval)
        {
            targetLight.enabled = !targetLight.enabled;
            timer = 0f;
        }

        // More control over the flicker
        //targetLight.intensity = Mathf.Lerp(minIntesity, maxIntesity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
        // PerlinNoise calculates things from one point to another and looks at a table of gradients
        // Feel free to look it up if your confused, I am...
    }
}
