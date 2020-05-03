using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVol : MonoBehaviour
{
    public AudioMixer aMixer;

    public void SetLevel(float sliderVal)
    {
        aMixer.SetFloat("MusicVol", Mathf.Log10(sliderVal) * 20);
    }
}
