using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnTeleporter : MonoBehaviour
{
    public GameObject playerPrefab;
    public static GameObject player;
    public GameObject particles;

    private void Awake()
    {
        if(player)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

        }
        else
        {
            player = Instantiate(playerPrefab, transform.position, transform.rotation);
        }
    }
}