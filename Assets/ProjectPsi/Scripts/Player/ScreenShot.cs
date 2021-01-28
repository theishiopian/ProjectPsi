using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ScreenShot : MonoBehaviour
{
    public SteamVR_Action_Boolean trigger;
    public SteamVR_Input_Sources rightController;


    private void Update()
    {
        if(trigger.GetStateDown(rightController))
        {
            ScreenCapture.CaptureScreenshot("Screenshots/" + System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".png");
            Debug.Log("snap");
        }
    }
}
