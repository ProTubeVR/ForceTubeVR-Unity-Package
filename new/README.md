# ProTubeVR's Unity Package

This repository contains a Unity App exemple that describes how to interact with the forcetubes in your game (should it be on PC or an Android device).

As this this an example app, you should only need the ForceTubeVRInteface.cs file (you can make you own though) and the libraries (.aar file in the "Android" folder and the .dll file in the "Windows" folder) that are located in the Assets/Plugins/ folder

Here are the functions that are usefull to you :

**```public static void InitAsync(bool pistolsFirst = false) ```**

    You must call this function first in order to connect to the ForceTubes. As the paired ForceTubes connect to your app/game, they will be dispatched in several channels in a specific order :
    RifleButt -> RifleBolt -> Pistol1 -> Pistol 2 -> Vest -> Other 

    If pistolFirst is true the order will be : 
    Pistol1 -> Pistol 2 -> RifleButt -> RifleBolt -> Vest -> Other 

    Those are the channels you'll be targeting with the other functions. You can also call the All channel (calling every channel) and the Rifle channel (calling RifleButt and RifleBolt)



**```public static void Kick(Byte power, ForceTubeVRChannel target = ForceTubeVRChannel.rifle) ```**

    This function allows you to send a kick request to the selected channel, by default it targets the rifle channel (rifleButt and rifleBolt), it takes a value between 0 and 255


**```public static void Rumble(Byte power, float duration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle) ```**

    This function send a rumble request to the targeted channel, it takes a power between 0 and 255 and a duration in ms between 0 and 500ms

**```public static void Shoot(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle) ```**

    This function calls the Kick and Rumble functions 

**```public static void SetActiveResearch(bool active) ```**

    This function enables or disables the bluetooth research (active by default, you don't need to call it when the app starts), set to false in order to stop trying to connecte forcetubes to your app

**```public static Byte TempoToKickPower(float tempo) ```**

    Use this function to get the maximum kick power according to the duration ("tempo") between two shots.

    You can't always put kick power to 255 (100%) because if your tempo is too short, the ForceTubeVR motor won't have the time to reset itself between two shots and you will have some loss of kicks. 

    It is useful if you want to make an autoshot with the higher kick powrer without loss of kicks.

    You can use this function in connection with Kick and Shot functions

**```public static Byte GetBatteryLevel() ```**

    This function gives you the battery value of the connected ForceTubeVR.

    You will obtain an unsigned byte, representing the percent of battery, so it's a value between 0 and 100.

    This function is obsolete because it always give the battery level from the first ForceTubeVR used. Now this plugin can manage more than one ForceTubeVR, it can be source of problems.


**```public static string ListConnectedForceTube() ```**

    This function returns a JSON object (as a string) that contains every connected ForceTubeVR

**```public static string ListChannels() ```**

    This function returns a JSON object (as a string) thats contains all channels and their linked ForceTubes 



**```public static bool SaveChannelJSon() ```**

    This function saves the current channels in a JSON object


**```public static bool LoadChannelJSon() ```**

    This function loads channels from the JSON

**```public static bool AddToChannel(int nChannel,string sName) ```**

    This function puts a ForceTube in a specific channel (it won't remove it from any other channel)

**```public static bool RemoveFromChannel(int nChannel, string sName) ```**

    This function removes a ForceTube from a specific channel

**```public static void ClearChannel(int nChannel) ```**

    This function empties a channel (removing every ForceTube in it)

**```public static void ClearAllChannel() ```**

    This function empties all channels

  
    
## **The functions bellow only work for an android device**

**```public static void OpenBluetoothSettings() ```**

    This function opens the Bluetooth settings of the device 

**```public static void DisconnectAll() ```**

    This function disconnectes all ForceTube from the device


