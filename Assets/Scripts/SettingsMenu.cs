using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_Text sensLabel;
    float origSens;
    Camera main;

    private void Awake()
    {
        main = Camera.main;
        origSens = main.GetComponent<cameraController>().sens;
        sensSlider.onValueChanged.AddListener(delegate { onSliderChange(sensSlider.value); });
    }

    // Slider 
    public void onSliderChange(float v)
    {
        main.GetComponent<cameraController>().sens = v;
        UpdateLabel(v);
    }

    // Apply method
    public void OnApply()
    {
        origSens = sensSlider.value;
        gameManager.instance.menuActive.SetActive(false);
        gameManager.instance.menuActive = gameManager.instance.menuPause;
        gameManager.instance.menuActive.SetActive(true);
    }

    // Back or return 
    public void OnBack()
    {
        main.GetComponent<cameraController>().sens = origSens;
        sensSlider.value = origSens;
        gameManager.instance.menuActive.SetActive(false);
        gameManager.instance.menuActive = gameManager.instance.menuPause;
        gameManager.instance.menuActive.SetActive(true);
    }

    // Update
    void UpdateLabel(float v)
    {
        if (sensLabel)
            sensLabel.text = "Sensitivity: {v:0}"; // Not sure if this is correct or not
    }
}
