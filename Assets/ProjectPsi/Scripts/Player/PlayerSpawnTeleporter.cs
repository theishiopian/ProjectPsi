using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnTeleporter : MonoBehaviour
{
    public static PlayerSpawnTeleporter instance;
    public GameObject fallbackPlayer;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    Transform player;

    private void Start()
    {
        if(!player)
        {
            player = GlobalVars.Get("player_rig").transform;
        }
        else
        {
            Destroy(fallbackPlayer);
            MovePlayer(transform);
        }
    }

    public void MovePlayer(Transform t)
    {
        player.position = t.position;
        player.rotation = t.rotation;
    }
}
