using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public VarInt bossMaxHP;
    public VarInt bossCurrHP;

    public static BossUI Instance;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Instance = this;
    }

    public void ReadyUI()
    {
        slider.maxValue = bossMaxHP.value;
        slider.value = bossCurrHP.value;
    }

    public void UpdateHealthBar()
    {
        slider.value = bossCurrHP.value;
    }
}
