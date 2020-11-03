using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnchor : MonoBehaviour
{
    private Transform hand;

    private void Start()
    {
        hand = GlobalVars.Get("right_hand").transform;
    }
    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.identity;
        this.transform.position = hand.position;
    }
}
