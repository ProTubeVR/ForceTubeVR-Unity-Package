using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ExampleScript : MonoBehaviour
{
    public Text batteryText;
    public Text DebugScan;
    public Dropdown m_db_toUpdate;
    private bool auto = false, activeResearch = true;
    private float autoTimer = 0.0f, autoDelay = 0.13f;
	private ForceTubeVRChannel target; //The example scene will send requests to this channel. The simplest is "all" (sending to all, ignoring the channel) and the channel rifle send requests to both "rifleButt" and "rifleBolt".
    private ForceTubeVRChannel ChannelToTest;
    private string sFT;
    //public ListForceTube ListFT;



    void Awake()
    {
		Debug.Log("InitAsync");
        ForceTubeVRInterface.InitAsync(false);

        //string path = Application.persistentDataPath;
        //string filePath = path+"/ForceChannel.json";
        //string dataAsJson = File.ReadAllText(filePath);
        //bool bRet = ForceTubeVRInterface.InitChannels(dataAsJson);

        ForceTubeVRInterface.LoadChannelJSon();
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

    public void UpdateConnectedList()
    {
        Debug.Log("UpdateConnectedList");
        //ListFT.value = "test";
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
		ForceTubeVRInterface.Shoot(255, 255, 0.5f, target);
    }
    public void ScanChannel()
    {
        string sText= ForceTubeVRInterface.ListChannels();
        Debug.Log("ListChannels " + sText);

        DebugScan.text = sText;
    }
    public void ScanConnected()
    {
        string sText = ForceTubeVRInterface.ListConnectedForceTube();
        Debug.Log("ListConnectedForceTube " + sText);

        DebugScan.text = sText;
    }

    public void InitChannelsTest()
    {
        string path = Application.persistentDataPath;
        string filePath = path + "/ForceChannel.json";
        Debug.Log("filePath : " + filePath);
        string dataAsJson = File.ReadAllText(filePath);

        bool bRet = ForceTubeVRInterface.InitChannels(dataAsJson);
        Debug.Log("ListConnectedForceTube " + bRet);        
    }


    public void SaveJSONChannelTest()
    {
        string sText = ForceTubeVRInterface.ListChannels();

        string path = Application.persistentDataPath;
        string filePath = path + "/ForceChannel.json";
        Debug.Log("filePath : " + filePath);
        File.WriteAllText(filePath,sText);
    }


    public void LoadJSONChannel()
    {
        /*string path = Application.persistentDataPath;
        string filePath = path + "/ForceChannel.json";
        Debug.Log("filePath : " + filePath);
        string dataAsJson = File.ReadAllText(filePath);

        bool bRet = ForceTubeVRInterface.InitChannels(dataAsJson);
        Debug.Log("ListConnectedForceTube " + bRet);*/



        ForceTubeVRInterface.LoadChannelJSon(); 
    }


    public void SaveJSONChannel()
    {
        /*string sText = ForceTubeVRInterface.ListChannels();

        string path = Application.persistentDataPath;
        string filePath = path + "/ForceChannel.json";
        Debug.Log("filePath : " + filePath);
        File.WriteAllText(filePath,sText);*/

        ForceTubeVRInterface.SaveChannelJSon();
    }


    public void SelectChannelToTest(int target)
    {
        this.ChannelToTest = (ForceTubeVRChannel)target+2;
    }

    public void AddToChannel()
    {
        string sForceTube = this.sFT;//this.m_db_toUpdate.GetComponent<Dropdown>().itemText.text;

        ForceTubeVRInterface.AddToChannel((int)this.ChannelToTest, sForceTube);
    }
    public void RemoveFromChannel()
    {
        string sForceTube = this.sFT;//this.m_db_toUpdate.GetComponent<Dropdown>().itemText.text;

        ForceTubeVRInterface.RemoveFromChannel((int)this.ChannelToTest, sForceTube);
    }

    public void ClearChannel()
    {
        ForceTubeVRInterface.ClearChannel((int)this.ChannelToTest);
    }

    public void SelectConnectedFT(int index )
    {
      
        this.sFT = this.m_db_toUpdate.GetComponent<Dropdown>().options[index].text;     
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

    public void OpenBluetoothSettings()
    {
        Debug.Log("Tyry open Bluetooth settings");
        ForceTubeVRInterface.OpenBluetoothSettings();
    }

    public void DisconnectAll()
    {
        ForceTubeVRInterface.DisconnectAll();
    }
}
