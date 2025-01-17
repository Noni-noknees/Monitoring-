using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f;    // Maximum energy level
    public float currentEnergy;       // Current energy level
    public float energyDrainRate = 1f; // Energy drain per second when camera or flashlight is active

    public Camera playerCamera;
    public Camera MainCamera; // Reference to the player's camera
    public Light flashlight;          // Reference to the flashlight light component

    private bool isCameraOn;
    private bool isMainCameraOn;
    private bool isFlashlightOn;
    public Image energybar;


    void Start()
    {
        currentEnergy = maxEnergy; // Start with full energy
    }

    void Update()
    {
        isCameraOn = !playerCamera.enabled;
        isMainCameraOn = MainCamera.enabled;



        isFlashlightOn = flashlight.enabled;

        // Decrease energy if camera or flashlight is on
        if ((isCameraOn&& isMainCameraOn) || isFlashlightOn)
        {
            DecreaseEnergy(Time.deltaTime);
        }

        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    void DecreaseEnergy(float timeDelta)
    {
        currentEnergy -= energyDrainRate * timeDelta;

        if (currentEnergy <= 0f)
        {
            currentEnergy = 0f;
            flashlight.gameObject.SetActive(false);
        }
        energybar.fillAmount = currentEnergy/100;

    }


    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }
}
