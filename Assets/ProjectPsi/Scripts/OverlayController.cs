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
    private Vignette vinEffects;//not spelling that every time

    // Start is called before the first frame update
    void Start()
    {
        globalVolume = GetComponent<PostProcessVolume>();
        health = GlobalVars.playerHealth;
        psi = GlobalVars.playerPsi;
    }

    //float oldHealth = 100; //use this for damage indication

    // Update is called once per frame
    float hbIntensity = 0;

    void Update()
    {

        if(GetEffects())
        {
            //colorEffects.saturation.value = health.GetHealth();
            //colorEffects.contrast

            health.GetHealth();
            colorEffects.saturation.value = health.GetHealth().Remap(0, 100, -100, 0);

            colorEffects.contrast.value = psi.GetPsi().Remap(10, 100, 0, 100);

            hbIntensity = 0;
            if(health.GetHealth() < 30)//todo make scale with health loss
            {
                hbIntensity = 1;
            }
        }

        HeartBeat();
    }

    private bool GetEffects()
    {
        return
            globalVolume.profile.TryGetSettings(out colorEffects)
            && globalVolume.profile.TryGetSettings(out damageEffects)
            && globalVolume.profile.TryGetSettings(out vinEffects);
    }

    int phase = 0;
    //0 waiting
    //1 first beat rise
    //2 second beat rise
    //3 second beat fall
    float t = 0;

    //todo make heartbeat scale with health loss
    private void HeartBeat()
    {
        t += Time.deltaTime;

        switch (phase)
        {
            case 0://wait
                {
                    if(t >= 0.5f)
                    {
                        phase = 1;
                        t = 0;
                    }
                }
                break;
            case 1://rise1
                {
                    vinEffects.intensity.value = Mathf.Lerp(0, hbIntensity, t * 4);

                    if(t >= 0.25f)
                    {
                        phase = 2;
                        t = 0;
                    }
                }
                break;
            case 2://rise2
                {
                    vinEffects.intensity.value = Mathf.Lerp(0, hbIntensity, t * 4);

                    if (t >= 0.25f)
                    {
                        phase = 3;
                        t = 0;
                    }
                }
                break;
            case 3://fall2
                {
                    vinEffects.intensity.value = Mathf.Lerp(hbIntensity, 0, t * 2);

                    if (t >= 0.50f)
                    {
                        phase = 0;
                        t = 0;
                    }
                }
                break;
        }
    }
}
