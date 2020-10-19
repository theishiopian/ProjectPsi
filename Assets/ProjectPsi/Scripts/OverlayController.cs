using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class OverlayController : MonoBehaviour
{

    private PostProcessVolume globalVolume;
    private Health health;
    private Psi psi;
    private ColorGrading colorEffects;
    private ChromaticAberration damageEffects;

    // Start is called before the first frame update
    void Start()
    {
        globalVolume = GetComponent<PostProcessVolume>();
        health = GlobalVars.playerHealth;
        psi = GlobalVars.playerPsi;
    }

    float oldHealth = 100;

    // Update is called once per frame
    void Update()
    {

        if(globalVolume.profile.TryGetSettings(out colorEffects)&& globalVolume.profile.TryGetSettings(out damageEffects))
        {
            //colorEffects.saturation.value = health.GetHealth();
            //colorEffects.contrast

            health.GetHealth();
            colorEffects.saturation.value = health.GetHealth().ReMap(0, 100, -100, 0);

            colorEffects.contrast.value = psi.GetPsi().ReMap(10, 100, 0, 100);
        }
    }
}
