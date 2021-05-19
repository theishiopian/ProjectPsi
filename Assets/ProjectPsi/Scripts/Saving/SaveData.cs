using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition = new Vector3();

    public List<Vector3> enemyPositions = new List<Vector3>();

    public List<bool> enemyLife = new List<bool>();

    public List<bool> doorsLocked = new List<bool>();

    public List<Vector3> cardPositions = new List<Vector3>();

    public List<bool> checkpoints = new List<bool>();
}
