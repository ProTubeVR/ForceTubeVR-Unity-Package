using UnityEngine;

public class Example : MonoBehaviour {

    private bool activated = true, autoMode = false;
    private float timer = 0.0f, autoTempo = 0.070f;

    void Update () { // there are showed the 3 methods this plugin adds

        if (Input.GetKeyDown(KeyCode.K))
        {
            ForceTubeVR.Kick(255); // 0 = no power, 255 = max power, this function is linear
        }

        if (Input.GetKey(KeyCode.R))
        {
            ForceTubeVR.Rumble(126, 0.1f); // for power : 0 = no power, 255 = max power, this function is linear, if power <= 126, only the little motor is activated ; for timeInSeconds : 0.0f seconds is a special command that make the ForceTubeVR never stop the rumble
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ForceTubeVR.Shot(255, 255, 0.1f); // combination of kick and rumble methods ; rumble duration still be in seconds and still don't stop if you set this parameter at 0.0f
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            activated = !activated;
            ForceTubeVR.SetActiveResearch(activated);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            autoMode = true;
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            autoMode = false;
        }

        if (autoMode)
        {
            timer += Time.deltaTime;
            if (timer >= autoTempo) //if it's time tio shoot
            {
                timer -= autoTempo;
                ForceTubeVR.Shot(ForceTubeVR.TempoToKickPower(autoTempo), 255, 0.1f);  //shoot using TempoToKickPower to be sure kick power is at the maximum value without any loss of kicks
            }
        }
    }
}
