using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVol : MonoBehaviour
{
    public AudioMixer aMixer;
    [SerializeField]
    private VarFloat musicVolSO;

    public void Start()
    {
        aMixer.SetFloat("MusicVol", musicVolSO.value);
    }
    public void SetLevel(float sliderVal)
    {
        aMixer.SetFloat("MusicVol", Mathf.Log10(sliderVal) * 20);
        musicVolSO.value = Mathf.Log10(sliderVal) * 20;
    }
}
