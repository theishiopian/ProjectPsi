using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;

    public List<Vector3> enemyPositions;

    public List<bool> doorsUnlocked;

    public List<Vector3> cardPositions;
}
