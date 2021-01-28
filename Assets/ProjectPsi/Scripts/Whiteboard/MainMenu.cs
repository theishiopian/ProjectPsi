using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject options;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartChecked(string input)
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnOptionsChecked(string input)
    {
        options.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnExitChecked(string input)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
