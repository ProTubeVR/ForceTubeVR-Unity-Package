using UnityEngine;
using System;
using System.Runtime.InteropServices;


///<summary>
///Useful to target different ForceTubeVR.
///"all" send requests to all, ignoring the channel settings, and "rifle" send requests to both "rifleButt" and "rifleBolt".
///By default, InitAsync() make the first ForceTubeVR detected is placed in the channel "rifleButt", the second is placed in "rifleBolt", and following are placed in channels "pistol1", "pistol2", "other" and "vest".
///</summary>
public enum ForceTubeVRChannel : int { all, rifle, rifleButt, rifleBolt, pistol1, pistol2, other, vest };


public class ForceTubeVRInterface : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR

    private static AndroidJavaObject ForceTubeVRPlugin = null;
    private static AndroidJavaClass androidClass = null;

#endif

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)

	[DllImport("ForceTubeVR_API_x32", EntryPoint = "InitRifle")]
	private static extern void InitRifle_x32();

	[DllImport("ForceTubeVR_API_x32", EntryPoint = "InitPistol")]
	private static extern void InitPistol_x32();

    [DllImport("ForceTubeVR_API_x32", EntryPoint = "SetActive")]
    private static extern void SetActiveResearch_x32(bool active);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "KickChannel")]
	private static extern void Kick_x32(Byte power, ForceTubeVRChannel channel);
    
	[DllImport("ForceTubeVR_API_x32", EntryPoint = "RumbleChannel")]
	private static extern void Rumble_x32(Byte power, float timeInSeconds, ForceTubeVRChannel channel);

	[DllImport("ForceTubeVR_API_x32", EntryPoint = "ShotChannel")]
	private static extern void Shot_x32(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel channel);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "TempoToKickPower")]
    private static extern Byte TempoToKickPower_x32(float tempo);
    
    [DllImport("ForceTubeVR_API_x32", EntryPoint = "GetBatteryLevel")]
    private static extern Byte GetBatteryLevel_x32();
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "InitRifle")]
    private static extern void InitRifle_x64();

	[DllImport("ForceTubeVR_API_x64", EntryPoint = "InitPistol")]
	private static extern void InitPistol_x64();

    [DllImport("ForceTubeVR_API_x64", EntryPoint = "SetActive")]
    private static extern void SetActiveResearch_x64(bool active);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "KickChannel")]
	private static extern void Kick_x64(Byte power, ForceTubeVRChannel channel);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "RumbleChannel")]
	private static extern void Rumble_x64(Byte power, float timeInSeconds, ForceTubeVRChannel channel);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "ShotChannel")]
	private static extern void Shot_x64(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel channel);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "TempoToKickPower")]
    private static extern Byte TempoToKickPower_x64(float tempo);
    
    [DllImport("ForceTubeVR_API_x64", EntryPoint = "GetBatteryLevel")]
    private static extern Byte GetBatteryLevel_x64();

#endif

	///<summary>
	///As suggered, this method is asynchronous.
	///Only need to be called once to let the Dll manage the ForceTubeVR's connection. 
	///By default, InitAsync() place the first ForceTubeVR detected in the channel "rifleButt" and the second in "rifleBolt". 
	///If it receives a boolean true as first param, the first forcetubevr is placed in "pistol1" and the second in "pistol2". 
	///</summary>
	public static void InitAsync(bool pistolsFirst = false)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
			using (androidClass = new AndroidJavaClass("com.ProTubeVR.ForceTubeVRInterface.ForceTubeVRInterface"))
			{
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
		ForceTubeVRPlugin = new AndroidJavaObject("com.ProTubeVR.ForceTubeVRInterface.ForceTubeVRInterface", context, pistolsFirst);
			}
		#endif

		#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
			if (pistolsFirst) {
				if (IntPtr.Size == 8) { 
					InitPistol_x64 ();
				} else {
					InitPistol_x32 ();
				}
			} else {
				if (IntPtr.Size == 8) { 
					InitRifle_x64 ();
				} else {
					InitRifle_x32 ();
				}
			}
		#endif
	}
    
    ///<summary>
    ///0 = no power, 255 = max power, this function is linear.
    ///</summary>
	public static void Kick(Byte power, ForceTubeVRChannel target = ForceTubeVRChannel.rifle)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
				ForceTubeVRPlugin.Call("sendKick", power, (int)target);
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        if (IntPtr.Size == 8)
            {
				Kick_x64(power, target);
            } else {
				Kick_x32(power, target);
            }
        #endif
    }

    ///<summary>
    ///For power : 0 = no power, 255 = max power, if power is 126 or less, only the little motor is activated, this function is linear.
	///For timeInSeconds : 0.0f seconds is a special command that make the ForceTubeVR never stop the rumble.
    ///</summary>
	public static void Rumble(Byte power, float duration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
				ForceTubeVRPlugin.Call("sendRumble", power, (int) (duration * 1000.0f), (int)target);
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
				Rumble_x64(power, duration, target);
            } else {
				Rumble_x32(power, duration, target);
            }
        #endif
    }

    ///<summary>
    ///Combination of kick and rumble methods.
	///Rumble duration still be in seconds and still don't stop if you set this parameter at 0.0f.
    ///</summary>
	public static void Shoot(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
				ForceTubeVRPlugin.Call("sendShot", kickPower, rumblePower, (int)(rumbleDuration * 1000.0f), (int)target);
            }
        #endif

        #if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            if (IntPtr.Size == 8)
            { 
				Shot_x64(kickPower, rumblePower, rumbleDuration, target);
            } else {
				Shot_x32(kickPower, rumblePower, rumbleDuration, target);
            }
        #endif
    }

    ///<summary>
	///It is true by default.  
	///Set it to false prevent the DLL to make a thread regularly check for connections and (re)connect ForceTubeVR when paired.
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
    ///Take duration in seconds between two shots(for auto-shots) and give you the maximal kick power you can use without any loss. 
	///If you don't use it, you may have some loss of kick if kick power is too big in high shot frequencies.
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
    ///Return the battery level in percents. 
	///So it's an unsigned byte value between 0 and 100.
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

	///<summary>
	///Only in Android system, launch the bluetooth settings activity, if you want to let users connect their ForceTubeVR in your game. 
	///If isInVR is true, launch this activity in the Oculus Quest application dedicated to VR TV.
	///</summary>
    public static void OpenBluetoothSettings(bool isInVR)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (ForceTubeVRPlugin != null)
            {
                ForceTubeVRPlugin.Call("openBluetoothSettings", isInVR);
            }
        #endif
    }
}