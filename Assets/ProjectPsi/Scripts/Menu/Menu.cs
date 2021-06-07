using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void MenuEvent();
//todo generic menu system?
public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject pauseMenu;

    public GameObject resumeButton;

    public Transform volumeIndicator;

    public Transform[] volumeNotches;

    public TextMeshPro volumeText;

    public Transform snapIndicator;

    public Transform[] snapNotches;

    public TextMeshPro snapText;

    public MeshRenderer smoothMoveCheck;

    public bool pauseMode = false;
    public bool debugMode = false;

    public static event MenuEvent OnLoad;
    public static event MenuEvent OnFinishedLoad;

    public static event MenuEvent OnMain;
    public static event MenuEvent OnOptions;

    public static event MenuEvent OnVolumeChanged;
    public static event MenuEvent OnSnapChanged;
    public static event MenuEvent OnSmoothChanged;
    public static event MenuEvent OnAwesomeChanged;

    public static event MenuEvent OnResume;
    public static event MenuEvent OnPlay;
    public static event MenuEvent OnQuit;

    string filePath;
    string degreeSymbol = "°";

    void OnEnable()
    {
        OnLoad?.Invoke();
        if (debugMode)
        {
            Debug.LogError("Deleting player prefs");
            PlayerPrefs.DeleteAll();
        }
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
            Debug.LogError("volume key not found");
        }
        if (!PlayerPrefs.HasKey("smoothmove"))
        {
            Debug.LogError("smoothmove key not found");
            PlayerPrefs.SetInt("smoothmove", 1);
        }

        if (!PlayerPrefs.HasKey("snapangle"))
        {
            PlayerPrefs.SetInt("snapangle", 15);
            Debug.LogError("snapangle key not found");
        }

        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        MoveVolumeIndicator();

        filePath = Application.persistentDataPath + "/game_data.json";

        if (File.Exists(filePath))
        {
            Debug.Log("Save file located at " + filePath);
            resumeButton.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No save file located");
        }

        smoothMoveCheck.enabled = PlayerPrefs.GetInt("smoothmove") == 1 ? false : true;

        OnFinishedLoad?.Invoke();
    }

    public void ChangeVolume(string input)
    {
        OnVolumeChanged?.Invoke();
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

    public void ChangeSnap(string input)
    {
        OnSnapChanged?.Invoke();

        switch (input)
        {
            case "15":
                {
                    PlayerPrefs.SetInt("snapangle", 15);
                }
                break;
            case "30":
                {
                    PlayerPrefs.SetInt("snapangle", 30);
                }
                break;
            case "45":
                {
                    PlayerPrefs.SetInt("snapangle", 45);
                }
                break;
        }

        MoveSnapIndicator();
    }

    public void ToggleSmooth(string input)
    {
        OnSmoothChanged?.Invoke();
        switch (input)
        {
            case "true":
                {
                    PlayerPrefs.SetInt("smoothmove", 1);
                }
                break;
            case "false":
                {
                    PlayerPrefs.SetInt("smoothmove", 0);
                }
                break;
        }
    }

    public void Resume(string input)
    {
        OnResume?.Invoke();
        Pause.paused = false;
        SceneManager.LoadScene("Level1");
    }

    //string could be used for other levels
    public void Play(string input)
    {
        OnPlay?.Invoke();
        
        if(File.Exists(filePath))
        {
            Debug.Log("Deleting old save data");
            File.Delete(filePath);
        }
        else
        {
            Debug.Log("Starting fresh game");
        }

        SceneManager.LoadScene("Level1");
    }

    //also used to go there in the first place :D
    public void Return(string input)
    {
        switch(input)
        {
            case "fromMain":
                {
                    OnOptions?.Invoke();
                    mainMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(true);
                }
                break;
            case "fromOptions":
                {
                    OnMain?.Invoke();
                    if(pauseMode)
                    {
                        pauseMenu.SetActive(true);
                        optionsMenu.SetActive(false);
                    }
                    else
                    {
                        mainMenu.SetActive(true);
                        optionsMenu.SetActive(false);
                    }
                }
                break;
        }
    }

    public void Exit(string input)
    {
        if(!pauseMode)
        {
            OnQuit?.Invoke();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private int GetIndex()
    {
        int r = Mathf.FloorToInt((volumeNotches.Length - 1) * PlayerPrefs.GetFloat("volume"));
        //Debug.Log(r);
        return r;
    }

    private void MoveVolumeIndicator()
    {
        int i = GetIndex();
        if (i < 0) return;//bandaid, may not be needed
        snapIndicator.localPosition = new Vector3(snapNotches[i].localPosition.x, snapIndicator.localPosition.y, snapIndicator.localPosition.z);

        snapText.text = (PlayerPrefs.GetInt("snaptext")).ToString() + degreeSymbol;
    }

    private void MoveSnapIndicator()
    {
        int a = PlayerPrefs.GetInt("snapangle");
        int i = 0;

        //fuck you VS
        switch(a)
        {
            case 15: i = 0;break;
            case 30: i = 1;break;
            case 45: i = 2;break;
        }

        if (i < 0) return;//bandaid, may not be needed

        volumeIndicator.localPosition = new Vector3(volumeNotches[i].localPosition.x, volumeIndicator.localPosition.y, volumeIndicator.localPosition.z);

        volumeText.text = (PlayerPrefs.GetFloat("volume") * 100).ToString() + "%";
    }
}
