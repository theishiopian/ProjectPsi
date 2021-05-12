using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEvent
{
    public string key;
    public AudioClip clip;
}

[System.Serializable]
public class SoundParams
{
    public float volume;
    public float pitch;
}

public class OneshotManager : MonoBehaviour
{
    [Header("Settup Settings")]
    public GameObject prefab;
    public int initialSize = 20;

    [Header("Default Settings")]
    public SoundParams defaultSoundSettings;

    [Header("Sound Events")]
    public List<SoundEvent> events;

    public static OneshotManager instance;

    private GameObjectPool sourcePool;
    private Dictionary<string, AudioClip> sounds;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Initilizing Oneshot System...");
        instance = this;
        DontDestroyOnLoad(instance);
        sourcePool = new GameObjectPool(prefab, initialSize, transform);

        foreach(SoundEvent e in events)
        {
            sounds.Add(e.key, e.clip);
        }

        if(sounds.Count > 0) Debug.Log(sounds.Count + " sound events loaded");
        else Debug.LogWarning("No sound events loaded");
    }

    public void PlaySound(string key, Vector3 position, SoundParams settings = null)
    {
        AudioSource source = sourcePool.Activate(position, Quaternion.identity, ResetAudioSource).GetComponent<AudioSource>();

        AudioClip clip;
        bool hasClip = sounds.TryGetValue(key, out clip);

        if(hasClip)
        {
            if(settings != null)
            {
                source.volume = settings.volume;
                source.pitch = settings.pitch;
            }
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Non-existant sound event \"" + key + "\"");
        }
    }

    private void ResetAudioSource(GameObject source)
    {
        AudioSource toClean = source.GetComponent<AudioSource>();

        toClean.volume = defaultSoundSettings.volume;
        toClean.pitch = defaultSoundSettings.pitch;
    }
}
