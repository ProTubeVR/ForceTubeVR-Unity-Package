using UnityEngine;
using UnityEngine.UI;

public class ExampleScript : MonoBehaviour
{
    public Text batteryText;
    private ForceTubeVRInterface forcetubevr;
    private bool auto = false, activeResearch = true, bluetoothLaunched = false;
    private float autoTimer = 0.0f, autoDelay = 0.13f, launchBluetoothTimer = 0.0f, launchBluetoothDelay = 5.0f;
    
    void Update()
    {
        if (auto)
        {
            autoTimer += Time.deltaTime;
            if (autoTimer >= autoDelay)
            {
                autoTimer -= autoDelay;
                ForceTubeVRInterface.Shoot(255, 255, 0.5f);
            }
        }
        if (!bluetoothLaunched)
        {
            launchBluetoothTimer += Time.deltaTime;
            if (launchBluetoothTimer >= launchBluetoothDelay)
            {
                bluetoothLaunched = true;
                ForceTubeVRInterface.OpenBluetoothSettings(true);
            }
        }
        batteryText.text = "BatteryLevel : " + ForceTubeVRInterface.GetBatteryLevel().ToString() + "%";
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        ForceTubeVRInterface.Shoot(255, 255, 0.5f);
    }

    public void AutoShoot()
    {
        auto = !auto;
    }

    public void Kick()
    {
        Debug.Log("Kick");
        ForceTubeVRInterface.Kick(255);
    }

    public void Rumble()
    {
        Debug.Log("Kick");
        ForceTubeVRInterface.Rumble(255, 0.5f);
    }

    public void setActiveResearch()
    {
        activeResearch = !activeResearch;
        Debug.Log("set active research to " + activeResearch.ToString());
        ForceTubeVRInterface.SetActiveResearch(activeResearch);
    }

    public void BluetoothSettingsVR()
    {
        Debug.Log("Bluetooth settings");
        ForceTubeVRInterface.OpenBluetoothSettings(true);
    }

    public void BluetoothSettingsPhone()
    {
        Debug.Log("Bluetooth settings");
        ForceTubeVRInterface.OpenBluetoothSettings(false);
    }
}
