
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class ForceTubeVRInterface : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR

    private static AndroidJavaObject ForceTubeVRPlugin = null;
    private static AndroidJavaClass androidClass = null;

#endif

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)

    [DllImport("ForceTubeVR_API_x32", EntryPoint = "InitAsync")]
    private static extern void InitAsync_x32();

    [DllImport("ForceTubeVR_API_x32", EntryPoint = "SetActive")]
    private static extern void SetActiveResearch_x32(bool active);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "Kick")]
    private static extern void Kick_x32(Byte power);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "Rumble")]
    private static extern void Rumble_x32(Byte power, float timeInSeconds);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "Shot")]
    private static extern void Shot_x32(Byte kickPower, Byte rumblePower, float rumbleDuration);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "TempoToKickPower")]
    private static extern Byte TempoToKickPower_x32(float tempo);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "GetBatteryLevel")]
    private static extern Byte GetBatteryLevel_x32();
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "InitAsync")]
    private static extern void InitAsync_x64();
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "SetActive")]
    private static extern void SetActiveResearch_x64(bool active);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "Kick")]
    private static extern void Kick_x64(Byte power);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "Rumble")]
    private static extern void Rumble_x64(Byte power, float timeInSeconds);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "Shot")]
    private static extern void Shot_x64(Byte kickPower, Byte rumblePower, float rumbleDuration);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "TempoToKickPower")]
    private static extern Byte TempoToKickPower_x64(float tempo);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "GetBatteryLevel")]
    private static extern Byte GetBatteryLevel_x64();

    ///<summary>
    ///As suggered, this method is asynchronous ; only need to be called once (I call it here at line 25)
    ///</summary>
    private static void InitAsync()
    {
        if (IntPtr.Size == 8)
        { 
            InitAsync_x64();
        } else {
            InitAsync_x32();
        }
    }

#endif
    
    ///<summary>
    ///0 = no power, 255 = max power, this function is linear
    ///</summary>
    public static void Kick(Byte power)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                ForceTubeVRPlugin.Call("sendKick", power);
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        if (IntPtr.Size == 8)
            { 
                Kick_x64(power);
            } else {
                Kick_x32(power);
            }
        #endif
    }

    ///<summary>
    ///For power : 0 = no power, 255 = max power, if power is 126 or less, only the little motor is activated, this function is linear ; for timeInSeconds : 0.0f seconds is a special command that make the ForceTubeVR never stop the rumble
    ///</summary>
    public static void Rumble(Byte power, float duration)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                ForceTubeVRPlugin.Call("sendRumble", power, (int) (duration * 1000.0f));
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
                Rumble_x64(power, duration);
            } else {
                Rumble_x32(power, duration);
            }
        #endif
    }

    ///<summary>
    ///Combination of kick and rumble methods ; rumble duration still be in seconds and still don't stop if you set this parameter at 0.0f
    ///</summary>
    public static void Shoot(Byte kickPower, Byte rumblePower, float rumbleDuration)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                ForceTubeVRPlugin.Call("sendShot", kickPower, rumblePower, (int)(rumbleDuration * 1000.0f));
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
                Shot_x64(kickPower, rumblePower, rumbleDuration);
            } else {
                Shot_x32(kickPower, rumblePower, rumbleDuration);
            }
        #endif
    }

    ///<summary>
    ///It is true by default, set it to false prevent the DLL to make a thread regularly check for disconnections and reconnect the ForceTubeVR if needed
    ///</summary>
    public static void SetActiveResearch(bool active)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                ForceTubeVRPlugin.Call("setActiveResearch", active);
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
                SetActiveResearch_x64(active);
            } else {
                SetActiveResearch_x32(active);
            }
        #endif
    }

    ///<summary>
    ///Take duration in seconds between two shots(for auto-shots) and give you the maximal kick power you can use without any loss(in high shot frequencies, you will have some loss of kick if kick power is too big)
    ///</summary>
    public static Byte TempoToKickPower(float tempo)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                return ForceTubeVRPlugin.Call<Byte>("tempoToKickPower", tempo);
            } else {
                return 255;
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
                return TempoToKickPower_x64(tempo);
            } else {
                return TempoToKickPower_x32(tempo);
            }
        #endif
    }

    ///<summary>
    ///Return the battery level in percents (so it's an unsigned byte value between 0 and 100)
    ///</summary>
    public static Byte GetBatteryLevel()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                return ForceTubeVRPlugin.Call<Byte>("getBatteryPercent");
            } else {
                return 255;
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
                return GetBatteryLevel_x64();
            } else {
                return GetBatteryLevel_x32();
            }
        #endif
    }

    public static void OpenBluetoothSettings(bool isInVR)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                ForceTubeVRPlugin.Call("openBluetoothSettings", isInVR);
            }
        #endif
    }

    [RuntimeInitializeOnLoadMethod]
    private static void OnLoadRuntimeMethod() // called at RuntimeInitialize, so you don't have to worry about initialization
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            using (androidClass = new AndroidJavaClass("com.ProTubeVR.ForceTubeVRInterface.ForceTubeVRInterface"))
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                ForceTubeVRPlugin = new AndroidJavaObject("com.ProTubeVR.ForceTubeVRInterface.ForceTubeVRInterface", context);
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            InitAsync();
        #endif
    }
}