using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean pauseButton;
    public float pauseDelay = 5f;

    private const string scene = "PauseRoomScene";

    float t = 0;
    // Update is called once per frame
    void Update()
    {
        if(pauseButton.GetState(controller))
        {
            t += Time.deltaTime;

            if(t > pauseDelay)
            {
                LevelSaveManager.currentInstance.SaveGame();
                SceneManager.LoadScene(scene);
            }
        }
    }
}
