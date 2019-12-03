using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioMixer EffectsMixer;

    public void SetLevel(float sliderValue)
    {
        Mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) *20);
    }

    public void SetEffectsLevel(float sliderValue)
    {
        EffectsMixer.SetFloat("MyEffects", Mathf.Log10(sliderValue) * 20);
    }
}
