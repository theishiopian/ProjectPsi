using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("volume")) PlayerPrefs.SetFloat("volume", 1.0f);
        if (!PlayerPrefs.HasKey("smoothmove")) PlayerPrefs.SetInt("smoothmove", 1);

        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void EditVolume(string input)
    {
        switch(input)
        {
            case "low":
                {
                    AudioListener.volume = 0;
                    PlayerPrefs.SetFloat("volume", 0);
                }
                break;
            case "mid":
                {
                    AudioListener.volume = 0.5f;
                    PlayerPrefs.SetFloat("volume", 0.5f);

                }
                break;
            case "high":
                {
                    AudioListener.volume = 1;
                    PlayerPrefs.SetFloat("volume", 1);
                }
                break;
        }
    }

    public void ToggleSmoothMove(string input)
    {
        switch(input)
        {
            case "true":
                {
                    PlayerPrefs.SetInt("smoothmove", 1);
                }break;
            case "false":
                {
                    PlayerPrefs.SetInt("smoothmove", 0);
                }
                break;
        }
    }
}
