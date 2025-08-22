using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_Text sensLabel;
    [SerializeField] cameraController cam;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject settingsMenu;

    float origSens;

    private void OnEnable()
    {
        origSens = cam.GetSensitivity();

        sensSlider.SetValueWithoutNotify(origSens);
        UpdateLabel(origSens);
    }

    // Slider 
    public void onSliderChange(float v)
    {
        cam.SetSensitivity(v);
        UpdateLabel(v);
    }

    // Apply method
    public void OnApply()
    {
        origSens = sensSlider.value;
        settingsMenu.SetActive(false);
        menuPause.SetActive(true);
    }

    // Back or return 
    public void OnBack()
    {
        cam.SetSensitivity(origSens);
        settingsMenu.SetActive(false);
        menuPause.SetActive(true);
    }

    // Update
    void UpdateLabel(float v)
    {
        if (sensLabel)
            sensLabel.text = "Sensitivity: {v:0}"; // Not sure if this is correct or not
    }
}
