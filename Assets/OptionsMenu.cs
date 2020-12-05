using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EditVolume(string input)
    {
        switch(input)
        {
            case "low":
                {
                    AudioListener.volume = 0;
                }break;
            case "mid":
                {
                    AudioListener.volume = 0.5f;
                }
                break;
            case "high":
                {
                    AudioListener.volume = 1;
                }
                break;
        }
    }
}
