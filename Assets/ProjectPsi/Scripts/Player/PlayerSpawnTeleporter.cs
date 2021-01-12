using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnTeleporter : MonoBehaviour
{
    public static PlayerSpawnTeleporter instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    Transform player;

    private void Start()
    {
        player = GlobalVars.Get("player_rig").transform;
    }

    public void MovePlayer(Transform t)
    {
        player.position = t.position;
        player.rotation = t.rotation;
    }
}
