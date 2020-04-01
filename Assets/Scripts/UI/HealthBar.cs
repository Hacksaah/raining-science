using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public VarInt maxHP;
    public VarInt currHP;

    public Text currHpText;
    public Text maxHpText;

    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();        
    }

    public void UpdateCurrHP()
    {
        slider.value = currHP.value;
        currHpText.text = currHP.value.ToString();
    }

    public void UpdateMaxHP()
    {
        slider.maxValue = maxHP.value;
        maxHpText.text = maxHP.value.ToString();
    }

    public void ReadyUI()
    {
        slider.maxValue = maxHP.value;
        slider.value = currHP.value;

        currHpText.text = currHP.value.ToString();
        maxHpText.text = maxHP.value.ToString();
    }
}
