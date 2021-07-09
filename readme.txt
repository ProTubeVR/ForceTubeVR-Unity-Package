/!\ InitAsync is the first function you need to call in order to work with the ForceTube.


Fonctions:

	ForceTubeVR void InitAsync(bool pistolsFirst = false);

	ForceTubeVR void Kick(Byte power, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);

	ForceTubeVR void Rumble(Byte power, float timeInSeconds, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);

	ForceTubeVR void Shot(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);

	ForceTubeVR void SetActiveResearch(bool active);

	ForceTubeVR Byte TempoToKickPower(float tempo);

	ForceTubeVR Byte GetBatteryLevel();

	ForceTubeVR void OpenBluetoothSettings();

units : 

power : value to be set from 0 to 255

kickPower : value to be set from 0 to 255 (side note : under 76 value (30%) the kick effect may be supper weak, this value will be setted depending on fire rate need ---> see kick description)

rumblePower : value to be set from 0 to 255

timeInSeconds : set rumble duration in second, 0.1 will activate rumble motor for 100ms (side note : under 0.03s, the motors are not activated enough to give any effect)

rumbleDuration : set rumble duration in second, 0.1 will activate rumble motor for 100ms (side note : under 0.03s, the motors are not activated enough to give any effect). You can make a rumble without end setting this parameter to 0.

target : channel to send the request. "all" redirect to all ForceTubeVR regardless of channels and "rifle" redirect to "rifleButt" and "rifleBolt" chanels.

active : True = the plugin will maintain a thread to watch the connection state of ForceTubeVR and reconnect if needed ; False = this thread will be desactivated ; it is set to True by default

tempo : duration in seconds between two shots(for auto-shots)

ForceTubeVRChannel : enum with values { all, rifle, rifleButt, rifleBolt, pistol1, pistol2, other, vest }. 

"all" and "rifle" aren't real channels because "all" redirect to all ForceTubeVR regardless of channels and "rifle" redirect to "rifleButt" and "rifleBolt" chanels.

-----------------------------------------------------------Connect--------------------------------------------------------------------------

Connect and pair the Forcetube,

On Windows :

The forcetube himself have to be paired to windows via bluetooth "standard connection" (see eventual specific tuto to pair it to windows).

On Android :

The minimum Android version supported is Android 4.4 "KitKat" so you shouldn't put a lower version for your Unity build setup.

The ForceTubeVR himself have to be paired to android via bluetooth "standard connection".

In standard phone, you can call the OpenBluetoothSettings method to open the android bluetooth manager activity in 2D and all is fine.

So with oculus quest, you basically have two ways to do it :
- you can use the ForceTubeVR app "BT Manager by ProTubeVR" available in sidequest
- you can call the OpenBluetoothSettings method to open the android bluetooth manager activity. 
The problem is this activity don't go back to unity main activity after finish and return to oculus quest home (but you only have to call it once : the pairing remains indefinitely).
I advice you to don't call this method and to let the protubevr application manage it to don't confuse all players in these settings.


And,
if the forcetube isn't powered on game start, or was turned off during the game, or if the connection is lost for any other reason,
the plugin automatically reconnect the ForceTubeVR when Windows detect the loss of connection
as well you haven't paused the dedicated thread by calling SetActiveResearch(false).



----------------------------------------------------------InitAsync-----------------------------------------------------------------

If a forcetube is already paired on Windows or Android and powered, then all the forcetubevrs will pass from "paired" status to "connected" status (can see it in Windows Bluetooth manager) just after you call InitAsync().
With this method, the six first paired forcetubevr detected will be put in the six channels in this order : rifleButt, rifleBolt, pistol1, pistol2, other, vest. It is the best for rifle games.
If you prefer to set it up as a two pistol game (or any game with one weapon by hand), you should call InitAsync with a boolean "true" as only parameter (like this : InitAsync(true)), 
and the six first paired forcetubevr detected will be put in the six channels in this order : pistol1, pistol2, rifleButt, rifleBolt, other, vest.
In the exemple, InitAsync() is called at RuntimeInitialize, so it is when the game with this plugin is launched (from editor or package).


----------------------------------------------------------Haptic effects------------------------------------------------------------------------

ForceTubeVR integrate 2 main haptic effects :
Rumble and Kick

----------------------------------------------------------Rumble----------------------------------------------------------------

Rumble use 2 "standard" rumble motors, one "small" (high frequency little effect) and one "big" (for loud rumble effect),
Those 2 motors are driven by the same signal as following :
-if rumble "power" is from 0 to 49% (0 to 126), only the small will spin (you can modulate the little motor speed, little motor max speed is reatch at 127 power value)
-if rumble "power" is from 50% to 100% (127 to 255), Small motor is driven max speed, big motor spin according to "power" value (255 give the max speed for both motors)
Rumble duration and power can be freely setted up, it depends of your needs

Fonction using it :
	FORCETUBEVR void Rumble(Byte power, float timeInSeconds, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);
	FORCETUBEVR void Shot(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);

----------------------------------------------------------Kick-----------------------------------------------------------------


The kick uses a solenoid to direct push/kick the user's shoulder,
it's possible to modulate the solenoid's power draw according to your needs (kick power can be set from 0 to 255),
To make it possible, the ForceTube's firmweare modulate power draw timings of the solenoid (depends on "kickpower" lvl, the firmweare will activate the solenoid for a given amont of time)
the activation timing can go from 0 to 100ms,
100ms will give the maximum Kick effect possible (255 value) (use this for big semi auto weapons)
30ms will give the minimal "good enough" effect (76 value) (use this for super light LMGs as P90 to handle high fire rates)

How to set power per weapon :
As said before, activation timings can go from 0 to 100ms,
The solenoid himself need 30ms of pause between each Kick,
so "kick power" can be setted like this :

Kickpower=((WeaponFireRate-30ms)/100)*255

some raw exemples :
M1014:250ms = 100% ---> 255 value
SPAS12:250ms = 100% ---> 255 value
SKS:125ms = 95% ---> 242 value
M39EMR:111ms =81% ---> 206 value
AKM:100ms =70% ---> 178 value
G3:100ms = 70% ---> 178 value
Mk16:100ms =70% ---> 178 value
Mk17:100ms =70% ---> 178 value
AK74u:91ms =61% ---> 156 value
AUG:91ms =61% ---> 156 value
PKM:91ms =61% ---> 156 value
TAR21:91ms =61% ---> 156 value
SA80:83ms =53% ---> 135 value
SVD:83ms =53% ---> 135 value
G36:83ms =53% ---> 135 value
AK5C:83ms =53% ---> 135 value
M249:83ms =53% ---> 135 value
MP5:83ms =53% ---> 135 value
AK12:77ms =47% ---> 120 value
Mk18:77ms =47% ---> 120 value
M16A4:71ms =41% ---> 105 value
552:66ms =36% ---> 92 value
Famas:66ms =36% ---> 92 value
P90:62ms =32% ---> 82 value
VAL:62ms = 32% ---> 82 value
M40A5: bolt action = 100% ---> 255 value
SV98: bolt action = 100% ---> 255 value

Fonction using the kick :
	FORCETUBEVR void Kick(Byte power, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);
	FORCETUBEVR void Shot(Byte kickPower, Byte rumblePower, float rumbleDuration, ForceTubeVRChannel target = ForceTubeVRChannel.rifle);



----------------------------------------------------------TempoToKickPower-----------------------------------------------------------------

Use it to get the maximum kick power according to the duration ("tempo") between two shots. 
You can't always put kick power to 255 (100%) because if your tempo is too short, the ForceTubeVR motor won't have the time to reset itself between two shots and you will have some loss of kicks. 
It is useful if you want to make an autoshot with the higher kick powrer without loss of kicks.

You can use this fonction in connection with :

FORCETUBEVR void Kick(Byte power);
FORCETUBEVR void Shot(Byte kickPower, Byte rumblePower, float rumbleDuration);

----------------------------------------------------------GetBatteryLevel-----------------------------------------------------------------

Use it to get battery value of the connected ForceTubeVR
You will obtain an unsigned byte, representing the percent of battery, so it's a value between 0 and 100.
This function is obsolete because it always give the battery level from the first ForceTubeVR used. Now this plugin can manage more than one ForceTubeVR, it can be source of problems.


----------------------------------------------------------OpenBluetoothSettings-----------------------------------------------------------

Only on Android system, launch the default bluetooth settings activity. Does nothing on Windows.
This can be useful if you want to let users connect their ForceTubeVR directly in your game. 
