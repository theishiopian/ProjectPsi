using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerListener
{
    // Start is called before the first frame update
    void OnEnter(Collider other);
    void OnExit(Collider other);
    void OnStay(Collider other);
}
