using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class BattleMaster : MonoBehaviour
{
    public GameObject enemyParent;
    public List<GameObject> scientistCallersIn = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.Instance.SetVariableValue("GlobalScientistCallers", scientistCallersIn);
        StartCoroutine("startRoutine");
    }

    IEnumerator startRoutine()
    {
        yield return new WaitUntil(() => GlobalVars.Get("player_body") != null);
        enemyParent.SetActive(true);
        yield return null;
    }
}
