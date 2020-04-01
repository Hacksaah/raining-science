using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    public VarFloat currentReloadSpeed;

    public Slider slider;    

    private void Awake()
    {
        slider.gameObject.SetActive(false);
    }

    public void StartReload()
    {
        float time = currentReloadSpeed.value;
        slider.gameObject.SetActive(true);
        StartCoroutine(UpdateUI(time));
    }

    IEnumerator UpdateUI(float time)
    {
        slider.maxValue = time;
        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;
            slider.value = timer;
            yield return null;
        }
        slider.gameObject.SetActive(false);
    }
}
