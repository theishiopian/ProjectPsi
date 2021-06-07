using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EndText
{
    public float time;
    [TextArea()]
    public string text;
}

public delegate void GameEndEvent();

public class EndingSequence : MonoBehaviour, ITriggerListener
{
    public static event GameEndEvent OnGameEndStart;
    public static event GameEndEvent OnGameEndEnd;

    public DoorController toLock;
    public TextMeshPro text;
    public List<EndText> EndTexts;

    private const string scene = "MainMenu";
    bool started = false;

    public void OnEnter(Collider other)
    {
        if(!started)StartCoroutine(EndSequence());
    }

    public void OnExit(Collider other)
    {

    }

    public void OnStay(Collider other)
    {

    }

    IEnumerator EndSequence()
    {
        started = true;
        toLock.Lock();
        OnGameEndStart?.Invoke();

        yield return new WaitForSeconds(1);

        //loop text

        for(int i = 0; i < EndTexts.Count; i++)
        {
            text.text = EndTexts[i].text;

            yield return new WaitForSeconds(EndTexts[i].time);
        }

        yield return new WaitForSeconds(1);

        OnGameEndStart?.Invoke();

        SceneManager.LoadScene(scene);

        yield return null;
    }
}