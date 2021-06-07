using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public delegate void PauseEvent();

public class Pause : MonoBehaviour
{
    public static PauseEvent OnPause;
    public static bool paused = false;
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean pauseButton;
    public float pauseDelay = 5f;

    private const string scene = "PauseRoomScene";
    private bool canPause = true;
    float t = 0;

    private void Start()
    {
        EndingSequence.OnGameEndStart += DisablePause;
        EndingSequence.OnGameEndEnd += EnablePause;
    }

    public void EnablePause()
    {
        canPause = true;
    }

    public void DisablePause()
    {
        canPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!paused && pauseButton.GetState(controller) && canPause)
        {
            t += Time.deltaTime;

            if(t > pauseDelay)
            {
                OnPause?.Invoke();
                paused = true;
                LevelSaveManager.currentInstance.SaveGame();
                SceneManager.LoadScene(scene);
                t = 0;
            }
        }
    }
}
