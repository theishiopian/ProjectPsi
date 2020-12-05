using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OptionsMenu : MonoBehaviour
{
    public Transform volumeIndicator;

    public Transform[] volumeNotches;

    public TextMeshPro volumeText;

    public bool debugMode = false;

    void OnEnable()
    {
        if(debugMode)PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("volume")) PlayerPrefs.SetFloat("volume", 1);
        if (!PlayerPrefs.HasKey("smoothmove")) PlayerPrefs.SetInt("smoothmove", 1);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        //Debug.Log(PlayerPrefs.GetFloat("volume"));
        MoveVolumeIndicator();
        
    }
    
    private int GetIndex()
    {
        int r = Mathf.FloorToInt((volumeNotches.Length - 1) * PlayerPrefs.GetFloat("volume"));
        //Debug.Log(r);
        return r;
    }

    private void MoveVolumeIndicator()
    {
        volumeIndicator.localPosition = new Vector3(volumeNotches[GetIndex()].localPosition.x, volumeIndicator.localPosition.y, volumeIndicator.localPosition.z);

        volumeText.text = (PlayerPrefs.GetFloat("volume") * 100).ToString() + "%";
    }

    public void EditVolume(string input)
    {
        switch (input)
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


        MoveVolumeIndicator();
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
