using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerSpawnTeleporter.instance.MovePlayer(this.transform);
    }
}
