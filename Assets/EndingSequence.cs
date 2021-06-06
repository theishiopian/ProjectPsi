using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static event GameEndEvent OnGameEnd;
    public void OnEnter(Collider other)
    {
        StartCoroutine(EndSequence());
    }

    public void OnExit(Collider other)
    {

    }

    public void OnStay(Collider other)
    {

    }

    IEnumerator EndSequence()
    {
        //lock door
        //disable pause

        //loop text

        //enable pause
        //return to menu
        yield return null;
    }
}
