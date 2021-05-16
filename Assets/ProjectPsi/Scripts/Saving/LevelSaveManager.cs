using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.JsonUtility;

public class LevelSaveManager : MonoBehaviour
{
    public static LevelSaveManager currentInstance;//weak singleton

    [Header("Savable Objects")]
    public List<GameObject> enemies;
    public List<GameObject> keycards;
    public List<DoorPanel> doors;

    string saveFilePath;
    SaveData data;

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/game_data.json";
        currentInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(ReadFile())
        {
            LoadGame();
        }
        else
        {
            Debug.Log("no save file detected at " + saveFilePath + ", a new one will be created upon saving");
        }
    }

    public void SaveGame()
    {
        if(data == null)
        {
            Debug.Log("Save Sequence Initiated");
            data = new SaveData();

            Debug.Log("Saving Player Position");
            data.playerPosition = GlobalVars.Get("player_rig").transform.position;

            Debug.Log("Saving Enemy Positions");
            for (int i = 0; i < enemies.Count; i++)
            {
                data.enemyPositions.Add(enemies[i].transform.position);
            }

            Debug.Log("Saving Keycard Positions");
            for (int i = 0; i < keycards.Count; i++)
            {
                data.cardPositions.Add(keycards[i].transform.position);
            }

            Debug.Log("Saving Door Lock States");
            for (int i = 0; i < doors.Count; i++)
            {
                data.doorsLocked.Add(doors[i].locked);
            }

            Debug.Log("Checking for save file");
            if (!File.Exists(saveFilePath))
            {
                Debug.Log("Creating new save file at " + saveFilePath);
            }
            else
            {
                Debug.Log("Saving to " + saveFilePath);
            }

            WriteFile(data);
        }
    }

    public void LoadGame()
    {
        Debug.Log("Load sequence initiated...");

        Debug.Log("Loading Player");
        GlobalVars.Get("player_rig").transform.position = data.playerPosition;

        Debug.Log("Loading Enemies");
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = data.enemyPositions[i];
        }

        Debug.Log("Loading Keycards");
        for (int i = 0; i < keycards.Count; i++)
        {
            keycards[i].transform.position = data.cardPositions[i];
        }

        Debug.Log("Loading Door");
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].locked = data.doorsLocked[i];
        }
    }

    public bool ReadFile()
    {
        // Does the file exist?
        if (File.Exists(saveFilePath))
        {
            string rawData = File.ReadAllText(saveFilePath);

            data = FromJson<SaveData>(rawData);
            return true;
        }
        return false;
    }

    public void WriteFile(SaveData toSave)
    {
        string serializedData = ToJson(toSave);

        File.WriteAllText(saveFilePath, serializedData);
    }
}
