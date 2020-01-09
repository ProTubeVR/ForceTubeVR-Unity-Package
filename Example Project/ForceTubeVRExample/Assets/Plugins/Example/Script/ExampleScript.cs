using UnityEngine;
using UnityEngine.UI;


public class ExampleScript : MonoBehaviour
{
    public Text batteryText; 
    private bool auto = false, activeResearch = true;
    private float autoTimer = 0.0f, autoDelay = 0.13f;
	private ForceTubeVRChannel target; //The example scene will send requests to this channel. The simplest is "all" (sending to all, ignoring the channel) and the channel rifle send requests to both "rifleButt" and "rifleBolt".


    void Awake()
    {
		Debug.Log("InitAsync");
        ForceTubeVRInterface.InitAsync(false);
    }

    void Update()
    {
        if (auto)
        {
            autoTimer += Time.deltaTime;
            if (autoTimer >= autoDelay)
            {
                autoTimer -= autoDelay;
				ForceTubeVRInterface.Shoot(255, 255, 0.5f, target);
            }
        }
		if (ForceTubeVRInterface.GetBatteryLevel () == 255) {
			batteryText.text = "BatteryLevel : Not found.";
		} else {
			batteryText.text = "BatteryLevel : " + ForceTubeVRInterface.GetBatteryLevel().ToString() + "%";
		}
			
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
		ForceTubeVRInterface.Shoot(255, 255, 0.5f, target);
    }

    public void AutoShoot()
    {
        auto = !auto;
    }

    public void Kick()
    {
        Debug.Log("Kick");
		ForceTubeVRInterface.Kick(255, target);
    }

    public void Rumble()
    {
        Debug.Log("Rumble");
		ForceTubeVRInterface.Rumble(255, 0.5f, target);
    }

	public void SetTargetChannel(int target){
		this.target = (ForceTubeVRChannel) target;
	}

    public void SetActiveResearch()
    {
        activeResearch = !activeResearch;
        Debug.Log("Set active research to " + activeResearch.ToString());
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
